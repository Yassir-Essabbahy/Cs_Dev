using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;
    private int score;
    public TextMeshProUGUI scoreText;

    public void AddScore(int points)
    {
     score += points;
     scoreText.text = "Score: " + score.ToString();
    }

    void Awake()
    {
        instance = this;
    }
    void Start()
    {
        AddScore(0);
    }
}
