using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [Header("Movements")]
    public float speed = 3f;
    public int direction = 1;

    [Header("Status")]
    public bool isFrozen = false;
    private float currentFreezeTime = 0f;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (isFrozen)
        {
            currentFreezeTime -= Time.fixedDeltaTime;

            if (currentFreezeTime <= 0)
                isFrozen = false;

            rb.linearVelocity = Vector3.zero;
            return;
        }

        rb.linearVelocity = new Vector3(direction * speed, rb.linearVelocity.y, 0f);
    }

    public void Freeze(float duration)
    {
        isFrozen = true;
        currentFreezeTime = duration;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            direction *= -1;
        }
    }
}