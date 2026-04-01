using System.Collections;
using TMPro;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    public GameObject talkPanel;
    public GameObject choicePack;
    public TextMeshProUGUI dialogueText;
    public int lastChoiceIndex;

    [Header("Voice Sounds")]
    public AudioClip[] voiceSounds;      // Drag your sounds here in the Inspector
    public AudioSource audioSource;      // Drag an AudioSource component here

    private bool choiceMade = false;

    void Awake()
    {
        Instance = this;
        talkPanel.SetActive(false);
        choicePack.SetActive(false);
    }

    public IEnumerator ShowDialogue(string[] lines, int choiceAfterLine = -1)
    {
        talkPanel.SetActive(true);

        for (int i = 0; i < lines.Length; i++)
        {
            yield return StartCoroutine(TypeLine(lines[i]));
            yield return StartCoroutine(WaitForInput());

            if (i == choiceAfterLine)
            {
                yield return StartCoroutine(HandleChoices());
            }
        }

        // Stop sound when dialogue ends
        if (audioSource != null) audioSource.Stop();

        talkPanel.SetActive(false);
    }

    IEnumerator TypeLine(string line)
    {
        dialogueText.text = "";

        // Play a random voice sound when a new line starts
        PlayRandomVoice();

        foreach (char c in line)
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(0.04f);
        }

        // Stop the sound when the line finishes typing
        if (audioSource != null) audioSource.Stop();
    }

    void PlayRandomVoice()
    {
        if (audioSource != null && voiceSounds != null && voiceSounds.Length > 0)
        {
            AudioClip clip = voiceSounds[Random.Range(0, voiceSounds.Length)];
            audioSource.clip = clip;
            audioSource.loop = true;   // Loop so it keeps playing while text types
            audioSource.Play();
        }
    }

    IEnumerator WaitForInput()
    {
        while (!Input.GetMouseButtonDown(0)) yield return null;
    }

    IEnumerator HandleChoices()
    {
        choicePack.SetActive(true);
        choiceMade = false;

        while (!choiceMade)
        {
            yield return null;
        }

        choicePack.SetActive(false);
    }

    public void MakeChoice()
    {
        choiceMade = true;
    }

    public void MakeChoice(int index)
    {
        lastChoiceIndex = index;
        choiceMade = true;
    }
}