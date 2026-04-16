using UnityEngine;
using System.Collections;

public class CameraSwitcher : MonoBehaviour
{
    public CamerasManager manager;
    public GameObject[] cameras;
    public int currentCameraIndex = 0;

    public GameObject staticOverlay; // Drag your Noise Image here
public AudioSource audioSource;
public AudioClip staticSound;



    void Update()
    {
        
        for ( int i = 0 ; i < cameras.Length ; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                SetCameraActive(i);
                TriggerStatic();
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

    public void TriggerStatic()
{
    StartCoroutine(CameraSwitchEffect());
}

IEnumerator CameraSwitchEffect()
{
    // 1. Turn on the noise and play the sound
    staticOverlay.SetActive(true);
    audioSource.PlayOneShot(staticSound);

    // 2. Wait for a tiny fraction of a second (PSX style is fast!)
    yield return new WaitForSeconds(0.15f);

    // 3. Turn it back off
    staticOverlay.SetActive(false);
}
}
