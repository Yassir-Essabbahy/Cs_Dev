using UnityEngine;
using System.Collections;

public class AnomallySpawner : MonoBehaviour
{

    public GameObject[] childAnomalies;

    void Start()
    {
        childAnomalies = new GameObject[transform.childCount];

        for (int i = 0; i < transform.childCount; i++)
        {
            childAnomalies[i] = transform.GetChild(i).gameObject;
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

                
    }
}
