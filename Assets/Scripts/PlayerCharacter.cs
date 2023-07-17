using UnityEngine;

/// <summary>
/// Basic character pawn with basic movement and jumping.
/// Movement uses the camera's forward and right vectors to determine the direction to move in.
/// </summary>
public class Character : Pawn
{
    public float speed = 6.0f;
    public float turnSpeed = 100.0f;
    public float jumpHeight = 2.0f;
    public float gravity = -9.81f;
    private CharacterController controller;
    private Vector3 velocity;
    public bool isGrounded;
    public float groundCheckDistance = 0.1f;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    public override void ReceiveInput(Vector2 movementInput, Vector2 rotationInput, bool jumpInput)
    {
        isGrounded = IsGrounded();
        Vector3 direction = CalculateDirection(movementInput);
        RotateCharacter(direction, movementInput);
        MoveCharacter(direction);
        ApplyGravity();
        HandleJump(jumpInput);
    }

    public virtual Vector3 CalculateDirection(Vector2 movementInput)
    {
        // Get PlayerController from the base class
        PlayerController playerController = GetPlayerController();

        Vector3 direction = Vector3.zero;
        if (movementInput.y > 0)
        {
            Vector3 cameraForward = playerController.ActiveCamera.transform.forward;
            cameraForward.y = 0;
            direction = cameraForward.normalized;
        }
        else if (movementInput.y < 0)
        {
            Vector3 cameraBackward = -playerController.ActiveCamera.transform.forward;
            cameraBackward.y = 0;
            direction = cameraBackward.normalized;
        }

        if (movementInput.x != 0)
        {
            Vector3 cameraRight = playerController.ActiveCamera.transform.right;
            direction += cameraRight * movementInput.x;
            direction.Normalize();
        }
        return direction;
    }

    public virtual void RotateCharacter(Vector3 direction, Vector2 movementInput)
    {
        // Get PlayerController from the base class
        PlayerController playerController = GetPlayerController();

        if (movementInput != Vector2.zero)
        {
            Quaternion targetRotation = Quaternion.Euler(0, playerController.ActiveCamera.transform.eulerAngles.y, 0);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * turnSpeed);
        }
    }

    public virtual void MoveCharacter(Vector3 direction)
    {
        controller.Move(direction * speed * Time.deltaTime);
    }

    public virtual void ApplyGravity()
    {
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = 0f;
        }
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    public virtual void HandleJump(bool jumpInput)
    {
        if (jumpInput && isGrounded)
        {
            velocity.y += Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }

    public virtual bool IsGrounded()
    {
        RaycastHit hit;
        float sphereRadius = controller.radius;
        Vector3 sphereOrigin = transform.position + Vector3.up * sphereRadius;
        if (Physics.SphereCast(sphereOrigin, sphereRadius, -Vector3.up, out hit, groundCheckDistance))
        {
            return true;
        }
        return false;
    }
}
