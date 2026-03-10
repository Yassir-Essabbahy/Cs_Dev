using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public void Restart()
    {
        SceneManager.LoadScene("SampleScene");
        Time.timeScale = 1f;
    }

}
