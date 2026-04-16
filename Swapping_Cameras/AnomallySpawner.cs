using UnityEngine;
using System.Collections;

public class AnomallySpawner : MonoBehaviour
{
    public GameObject[] childAnomalies;

    public AudioClip audioClip;
    public AudioSource audioSource;

    // Reference to CameraSwitcher
    public CameraSwitcher cameraSwitcher;

    // The camera index that looks at this room
    public int blockedCameraIndex;

    void Start()
    {
        Debug.Log($"Found {transform.childCount} children");
        
        childAnomalies = new GameObject[transform.childCount];

        for (int i = 0; i < transform.childCount; i++)
        {
            childAnomalies[i] = transform.GetChild(i).gameObject;
            Debug.Log($"Assigned: {childAnomalies[i].name}");
        }

        StartCoroutine(AnomalyTimer());
    }

    IEnumerator AnomalyTimer()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(15, 30));

            // Don't spawn if player is looking at this camera
            if (cameraSwitcher.currentCameraIndex == blockedCameraIndex)
            {
                Debug.Log("Player looking here — skipping spawn");
                continue;
            }

            SpawnRandomAnomaly();
        }
    }

    void SpawnRandomAnomaly()
    {
        int i = Random.Range(0, childAnomalies.Length);

        childAnomalies[i].SetActive(true);

        if (audioSource && audioClip)
            audioSource.PlayOneShot(audioClip);

        Debug.Log("Anomaly Spawned");
    }

    public bool IsAnyAnomalyActive()
    {
        foreach (GameObject anomaly in childAnomalies)
        {
            if (anomaly.activeSelf)
                return true;
        }

        return false;
    }

    public void ClearAllAnomalies()
    {
        foreach (GameObject anomaly in childAnomalies)
        {
            anomaly.SetActive(false);
        }
    }
}