using UnityEngine;
using TMPro;

public class CamerasManager : MonoBehaviour
{
    public TextMeshProUGUI camNameDisplay;
    public TextMeshProUGUI strikeDisplay;

    int strikes = 0;

    public void UpdateCameraUI(string name)
    {
        camNameDisplay.text = name;
    }

    public void AddStrike()
    {
        strikes++;
        strikeDisplay.text = "STRIKES: " + strikes + "/3";
    }

}
