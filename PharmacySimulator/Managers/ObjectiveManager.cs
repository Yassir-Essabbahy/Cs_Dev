using UnityEngine;
using TMPro;

public class ObjectiveManager : MonoBehaviour
{
    public TextMeshProUGUI objectiveUI;

    public string[] objectives;

    private int currentStep = 0;

    void Start()
    {
        UpdateUI();
    }

    public void CompleteObjective()
    {
        currentStep++;

        if (currentStep < objectives.Length)
        {
            UpdateUI();
        }
        else
        {
            objectiveUI.text = "Shift Finished. Go home.";
        }

        
    }
    void UpdateUI()
    {
        objectiveUI.text = objectives[currentStep];
    }
}
