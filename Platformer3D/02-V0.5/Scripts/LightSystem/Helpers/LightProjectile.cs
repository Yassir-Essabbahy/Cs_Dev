using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class LightProjectile : MonoBehaviour
{
    [SerializeField] private float lifeTime = 3f;

    private Rigidbody rb;
    private Vector3 moveDirection = Vector3.forward;
    private float moveSpeed = 20f;
    private float freezeDuration = 2f;
    private GameObject ownerObject;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        Destroy(gameObject, lifeTime);
    }

    public void Initialize(Vector3 direction, float speed, float freezeTime, GameObject owner)
    {
        moveDirection = direction.normalized;
        moveSpeed = speed;
        freezeDuration = freezeTime;
        ownerObject = owner;

        if (rb != null)
        {
            rb.linearVelocity = moveDirection * moveSpeed;
        }
    }

    private void Start()
    {
        rb.linearVelocity = moveDirection * moveSpeed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (ownerObject != null && other.transform.root.gameObject == ownerObject)
        {
            return;
        }

        EnemyLightReaction enemy = other.GetComponentInParent<EnemyLightReaction>();
        if (enemy != null)
        {
            enemy.Freeze(freezeDuration);
            Destroy(gameObject);
            return;
        }

        if (!other.isTrigger)
        {
            Destroy(gameObject);
        }
    }
}
