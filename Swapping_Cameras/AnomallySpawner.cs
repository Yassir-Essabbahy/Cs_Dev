using UnityEngine;
using System.Collections;

public class AnomallySpawner : MonoBehaviour
{

    public GameObject[] childAnomalies;
    public AudioClip audioClip;
    public AudioSource audioSource;

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

            yield return new WaitForSeconds(Random.Range(5,15));

            SpawnRandomAnomaly();
        }
    }

    void SpawnRandomAnomaly()
    {
        int i = Random.Range(0,childAnomalies.Length);


        childAnomalies[i].SetActive(true);
        audioSource.PlayOneShot(audioClip);
        Debug.Log("Anomaly Spawned");

                
    }
}
