using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 6f;

    [Header("Jump & Gravity")]
    [SerializeField] private float jumpHeight = 1.8f;
    [SerializeField] private float gravity = -25f;

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheckPoint;
    [SerializeField] private float groundCheckRadius = 0.25f;
    [SerializeField] private LayerMask groundLayer;

    private CharacterController controller;
    private float horizontalInput;
    private float verticalVelocity;
    private bool isGrounded;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        CheckGround();
        ReadInput();
        HandleRotation();
        HandleJump();
        ApplyGravity();
        MovePlayer();
    }

    private void ReadInput()
    {
        // Only LEFT / RIGHT
        horizontalInput = Input.GetAxisRaw("Horizontal");
    }

    private void HandleRotation()
    {
        if (Mathf.Abs(horizontalInput) < 0.01f)
            return;

        // Face left or right
        if (horizontalInput > 0)
            transform.rotation = Quaternion.Euler(0, 90f, 0);
        else
            transform.rotation = Quaternion.Euler(0, -90f, 0);
    }

    private void HandleJump()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }

    private void ApplyGravity()
    {
        verticalVelocity += gravity * Time.deltaTime;
    }

    private void MovePlayer()
    {
        Vector3 motion = new Vector3(horizontalInput * moveSpeed, verticalVelocity, 0f);
        controller.Move(motion * Time.deltaTime);
    }

    private void CheckGround()
    {
        if (groundCheckPoint != null)
        {
            isGrounded = Physics.CheckSphere(
                groundCheckPoint.position,
                groundCheckRadius,
                groundLayer,
                QueryTriggerInteraction.Ignore
            );
        }
        else
        {
            isGrounded = controller.isGrounded;
        }

        if (isGrounded && verticalVelocity < 0f)
        {
            verticalVelocity = -2f;
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheckPoint == null) return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(groundCheckPoint.position, groundCheckRadius);
    }
}