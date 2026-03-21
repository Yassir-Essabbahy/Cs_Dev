using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    private CharacterController controller;
   [Header("Movements")]
   public float speed = 10f;
   public float turnTime = 0.1f;
   private float turnVelocity;

   [Header("Jumping ou Gravity")]
   public float jumpHeight = 2.5f;
   public float gravity = -25f;
   private Vector3 velocity;
   private bool isGrounded;

   void Awake()
    {
        controller = GetComponent<CharacterController>();
    }
    void Update()
   {
        isGrounded = controller.isGrounded;
        if(isGrounded && velocity.y < 0) velocity.y = -2f;

        // Get Input
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal,0f,vertical).normalized;

        // Movement Logic
        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnVelocity, turnTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * speed * Time.deltaTime);
        }

        // Jump Logic
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // Gravity Logic
        velocity.y = velocity.y + gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
   }
}
