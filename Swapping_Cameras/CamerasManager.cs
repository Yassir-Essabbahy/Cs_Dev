using UnityEngine;
using TMPro;

public class CamerasManager : MonoBehaviour
{
    public AnomallySpawner[] roomSpawners;
    public CameraSwitcher cameraSwitcher;

    public int currentStreak = 0;
    public int winRequirement = 8;
    public int maxStrikes = 3;
    public int strikes = 0;

    bool isGameOver = false;

    public GameObject winPanel;
    public GameObject losePanel;

    public TextMeshProUGUI camNameDisplay;
    public TextMeshProUGUI strikeDisplay;


    public void UpdateCameraUI(string name)
    {
        camNameDisplay.text = name;
    }

    public void UpdateStrikeUI()
    {
        strikeDisplay.text = "STRIKES: " + strikes + "/3";
    }

    public void OnReportButtonPressed()
    {
        if (isGameOver) return;

        if (roomSpawners[cameraSwitcher.currentCameraIndex].IsAnyAnomalyActive())
        {
            currentStreak++;
            Debug.Log("Correct! Anomaly Cleared. streak : " + currentStreak);
            roomSpawners[cameraSwitcher.currentCameraIndex].ClearAllAnomalies();

            if (currentStreak >= winRequirement)
            {
                WinGame();
            }
        }
        else
        {
            strikes++;
            currentStreak = 0;
            Debug.Log("Wrong! that's a strike");
            UpdateStrikeUI();

            if (strikes >= maxStrikes)
            {
                LoseGame();
            }
        }
    }

    void WinGame()
    {
        isGameOver = true;
        winPanel.SetActive(true);
        Time.timeScale = 0;
        Debug.Log("You win");
    }

    void LoseGame()
    {
        isGameOver = true;
        losePanel.SetActive(true);
        Time.timeScale = 0;
        Debug.Log("You're fired");
    }

    

}
