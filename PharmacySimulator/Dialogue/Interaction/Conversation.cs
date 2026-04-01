using System.Collections;
using UnityEditor.EditorTools;
using UnityEngine;

public class Conversation : MonoBehaviour
{
    public string[] lines;
    public string[] repeatLines;
    public bool hasTalkedBefore = false;
    [Tooltip("Change to the next objective")]
    public bool changeObjective = true;

    [Tooltip("Show the choice panel after this line index. Set to -1 for no choice.")]
    public int choiceAfterLine = -1;

    [Tooltip("Drag the NPC's Head bone here")]
    public Transform headTarget;

    [Header("Prescription Settings")]
    public bool givesPrescription = false;

    [Tooltip("What does the paper say?")]
    public string patientName;
    public string drugRequested;
    public string dateWritten;
    public string reasonGiven;

    public IEnumerator Play()
    {
        if (hasTalkedBefore == true)
        {
        
        yield return StartCoroutine(DialogueManager.Instance.ShowDialogue(repeatLines, -1));
        
        
        
        }
        else
        {
            
        yield return StartCoroutine(DialogueManager.Instance.ShowDialogue(lines, choiceAfterLine));
            if (changeObjective)
            {
        FindObjectOfType<ObjectiveManager>().CompleteObjective();
                
            }
        hasTalkedBefore = true;
        }

        if (givesPrescription)
        {
            PrescriptionSystem ps = FindObjectOfType<PrescriptionSystem>();

            if (ps != null)
            {
                ps.ShowPrescription(patientName, drugRequested, dateWritten, reasonGiven);
            }
            else
            {
                Debug.LogWarning("Couldn't Find The PrescriptionSystem in the scene");
            }
        }

        
    }
}