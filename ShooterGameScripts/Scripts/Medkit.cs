using UnityEngine;

public class Medkit : MonoBehaviour
{
    public int healAmount = 10;

    void OnTriggerEnter(Collider other)
    {
        PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.Heal(healAmount);
            Destroy(gameObject);
        }
    }
}
