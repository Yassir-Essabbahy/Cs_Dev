using UnityEngine;

public class Coin : MonoBehaviour
{
    public sms_testing1 smsScript;
    void OnTriggerEnter(Collider other)
    {
        ScoreManager.instance.AddScore(10);
        smsScript.Test();
        
        Destroy(gameObject);
    }
}
