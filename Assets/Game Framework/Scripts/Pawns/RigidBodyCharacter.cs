using UnityEngine;

/// <summary>
/// Basic character pawn with basic movement and jumping.
/// Movement uses the camera's forward and right vectors to determine the direction to move in.
/// </summary>
public class RigidBodyCharacter : Pawn
{
    [Header("Movement Settings")]
    public float walkSpeed = 6.0f;
    public float runSpeed = 10f;
    public float airControlFactor = .2f;
    public float turnSpeed = 100.0f;

    [Header("Jump Settings")]
    public float jumpForce = 2.0f;

    [Header("Gravity Settings")]
    public float gravity = -9.81f;

    [Header("Keys")]
    public KeyCode sprintKey = KeyCode.LeftShift;

    [Header("Check Variables")]
    private Vector3 velocity;
    public bool isGrounded;
    public float groundCheckDistance = 0.2f;

    [Header("Physics")]
    private Rigidbody rigidBody;
    public float damping = 10f;

    [Header("Direction")]
    private Vector3 direction;
    private Vector2 movement;
    private bool jumpInput;
    private Vector3 combinedRaycast;

    [Header("Timers and Speeds")]
    public float airControlTime = 0.5f;
    public float airControlTimer = 0f;
    public float currentSpeed = 0f;

    [Header("Misc")]
    public bool lockRotation = false;

    public override void Start()
    {
        base.Start();
        rigidBody = GetComponent<Rigidbody>();
    }

    public override void ReceiveInput(Vector2 movementInput, Vector2 rotationInput, bool jumpInput)
    {
        if (!isPossessed)
            return;
        this.jumpInput = jumpInput;
        movement = movementInput;
        direction = CalculateDirection(movementInput);

        if (movementInput == Vector2.zero)
        {
            direction = Vector3.zero;
        }

        bool wasGrounded = isGrounded;
        isGrounded = IsGrounded();

        // updating air control timer here
        if (!wasGrounded && isGrounded)
            airControlTimer = 0;
        else if (!isGrounded)
            airControlTimer += Time.deltaTime;

        RotateCharacter(direction, movementInput);
        MoveCharacter(direction);
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
        if (lockRotation)
        {
            Quaternion targetRotation = Quaternion.Euler(0, this.PlayerController.PossessedCamera.transform.eulerAngles.y, 0);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * turnSpeed);
            return;
        }

        if (movementInput != Vector2.zero)
        {
            Quaternion targetRotation = Quaternion.Euler(0, this.PlayerController.PossessedCamera.transform.eulerAngles.y, 0);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * turnSpeed);
        }
    }

    public virtual void MoveCharacter(Vector3 direction)
    {
        currentSpeed = Input.GetKey(sprintKey) ? runSpeed : walkSpeed;

        // reduce speed if in air
        if (!isGrounded)
        {
            if (airControlTimer > airControlTime)
            {
                airControlTimer = airControlTime;
                currentSpeed = 0f;
                return;

            }
            currentSpeed *= airControlFactor;

        }

        // calculate force to apply
        Vector3 force = direction * currentSpeed - rigidBody.velocity;
        force.y = 0; // ignore vertical force
        // apply force
        rigidBody.AddForce(force, ForceMode.VelocityChange);
    }


    public virtual void HandleJump(bool jumpInput)
    {
        if (jumpInput && isGrounded)
        {
            airControlTimer = 0;
            rigidBody.AddForce(new Vector3(0, jumpForce, 0), ForceMode.Impulse);
        }
    }

    public virtual bool IsGrounded()
    {
        RaycastHit hit;
        Collider collider = GetComponent<Collider>();
        float sphereRadius = collider.bounds.extents.y;
        Vector3 sphereOrigin = transform.position + Vector3.up * (sphereRadius + 0.05f); // Start just above the bottom of the collider

        if (Physics.SphereCast(sphereOrigin, sphereRadius, -Vector3.up, out hit, groundCheckDistance + 0.05f)) // Cast downwards
        {
            return true;
        }
        else
        {
            return false;

        }
    }

    public virtual RaycastHit CheckGround()
    {
        RaycastHit hit;
        Collider collider = GetComponent<Collider>();
        float sphereRadius = collider.bounds.extents.y;
        Vector3 sphereOrigin = transform.position + Vector3.up * (sphereRadius + 0.05f); // Start just above the bottom of the collider

        if (Physics.SphereCast(sphereOrigin, sphereRadius, -Vector3.up, out hit, groundCheckDistance + 0.05f)) // Cast downwards
        {
            return hit;
        }
        else
        {
            return hit;
        }
    }
}
