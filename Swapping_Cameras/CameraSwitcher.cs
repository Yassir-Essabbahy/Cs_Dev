using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    public CamerasManager manager;
    public GameObject[] cameras;
    public int currentCameraIndex = 0;



    void Update()
    {
        
        for ( int i = 0 ; i < cameras.Length ; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                SetCameraActive(i);
                manager.UpdateCameraUI("CAM 0"+ (i+1));
            }
        }
    }

    void SetAllActive(bool State)
    {
        foreach (GameObject cam in cameras)
        {
            if (cam != null)
            {
                cam.SetActive(State);
            }
        }
    }
    
    void SetCameraActive(int CameraIndex)
    {

        if (CameraIndex != currentCameraIndex)
        {
        SetAllActive(false);
            
        cameras[CameraIndex].SetActive(true);

        currentCameraIndex = CameraIndex;
        }
    }
}
