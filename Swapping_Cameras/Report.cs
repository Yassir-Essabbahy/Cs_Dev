using UnityEngine;

public class Report : MonoBehaviour
{
    public CamerasManager manager;
    public CameraSwitcher cameraSystem;
    public void OnReportButtonPressed()
    {
        Debug.Log("Report Button Pressed : cam " + cameraSystem.currentCameraIndex+1);
        manager.AddStrike();    
    }
}
