using UnityEngine;

/// <summary>
/// Basic character pawn with movement and jumping.
/// Movement uses the camera's forward and right vectors to determine the direction to move in.
/// </summary>
public class Character : Pawn
{
    [Header("Movement")]
    public float walkSpeed = 6.0f;
    public float runSpeed = 10f;
    public float turnSpeed = 100.0f;

    [Header("Air Control")]
    public float airControlFactor = .2f;
    public float jumpHeight = 2.0f;

    [Header("Physics")]
    public float gravity = -9.81f;
    public float groundCheckDistance = 0.1f;

    [Header("Input")]
    public KeyCode sprintKey = KeyCode.LeftShift;

    [Header("Controller")]
    private CharacterController controller;
    private Vector3 velocity;
    public bool isGrounded;



    public override void Start()
    {
        base.Start();
        controller = GetComponent<CharacterController>();
    }

    public override void ReceiveInput(Vector2 movementInput, Vector2 rotationInput, bool jumpInput)
    {
        if (!isPossessed)
            return;

        isGrounded = IsGrounded();
        Vector3 direction = CalculateDirection(movementInput);
        if (movementInput == Vector2.zero)
        {
            direction = Vector3.zero;
        }
        RotateCharacter(direction, movementInput);
        MoveCharacter(direction);
        ApplyGravity();
        HandleJump(jumpInput);
    }

    public virtual Vector3 CalculateDirection(Vector2 movementInput)
    {

        Vector3 direction = Vector3.zero;
        if (movementInput.y > 0)
        {
            Vector3 cameraForward = this.PlayerController.PossessedCamera.transform.forward;
            cameraForward.y = 0;
            direction = cameraForward.normalized;
        }
        else if (movementInput.y < 0)
        {
            Vector3 cameraBackward = -this.PlayerController.PossessedCamera.transform.forward;
            cameraBackward.y = 0;
            direction = cameraBackward.normalized;
        }

        if (movementInput.x != 0)
        {
            Vector3 cameraRight = this.PlayerController.PossessedCamera.transform.right;
            direction += cameraRight * movementInput.x;
            direction.Normalize();
        }
        return direction;
    }

    public virtual void RotateCharacter(Vector3 direction, Vector2 movementInput)
    {
        // Get PlayerController from the base class

        if (movementInput != Vector2.zero)
        {
            Quaternion targetRotation = Quaternion.Euler(0, this.PlayerController.PossessedCamera.transform.eulerAngles.y, 0);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * turnSpeed);
        }
    }

    public virtual void MoveCharacter(Vector3 direction)
    {
        float currentSpeed = Input.GetKey(sprintKey) ? runSpeed : walkSpeed;

        // reduce speed if in air
        if (!isGrounded)
        {
            currentSpeed *= airControlFactor; // airControlFactor < 1, e.g., 0.2f
        }

        // walk
        Vector3 horizontalMove = direction * currentSpeed * Time.deltaTime;
        controller.Move(horizontalMove);
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
        float sphereRadius = controller.radius;
        Vector3 sphereOrigin = transform.position + Vector3.up * sphereRadius;
        if (Physics.SphereCast(sphereOrigin, sphereRadius, -Vector3.up, out RaycastHit hit, groundCheckDistance))
        {
            return true;
        }
        return false;
    }
}
