using UnityEngine;

public class Coin : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        ScoreManager.instance.AddScore(10);
        Destroy(gameObject);
    }
}
