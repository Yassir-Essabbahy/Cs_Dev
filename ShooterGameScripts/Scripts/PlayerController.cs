using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class RigidbodyMovement3D : MonoBehaviour
{
    public float moveSpeed = 6f;
    public float rotationSpeed = 10f;

    private Rigidbody rb;
    private Vector3 moveInput;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        moveInput = new Vector3(h, 0f, v).normalized;
    }

    void FixedUpdate()
    {
        Vector3 move = moveInput * moveSpeed;

        rb.linearVelocity = new Vector3(move.x, rb.linearVelocity.y, move.z);

        if (moveInput != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveInput);
            rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime));
        }
    }
}