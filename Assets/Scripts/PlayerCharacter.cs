using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    public float speed = 6.0f;
    public float turnSpeed = 100.0f;
    public float jumpHeight = 2.0f;
    public float gravity = -9.81f;
    private CharacterController controller;
    private Vector3 velocity;
    private PlayerController playerController;
    public bool isGrounded;
    public float groundCheckDistance = 0.1f;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        playerController = GameMode.Instance.GetPlayerController();
    }

    public virtual void ReceiveInput(Vector2 movementInput, Vector2 rotationInput, bool jumpInput)
    {
        isGrounded = IsGrounded();
        Vector3 direction = CalculateDirection(movementInput);
        RotateCharacter(direction, movementInput);
        MoveCharacter(direction);
        ApplyGravity();
        HandleJump(jumpInput);
    }

    Vector3 CalculateDirection(Vector2 movementInput)
    {
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

    void RotateCharacter(Vector3 direction, Vector2 movementInput)
    {
        if (movementInput != Vector2.zero)
        {
            Quaternion targetRotation = Quaternion.Euler(0, playerController.ActiveCamera.transform.eulerAngles.y, 0);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * turnSpeed);
        }
    }

    void MoveCharacter(Vector3 direction)
    {
        controller.Move(direction * speed * Time.deltaTime);
    }

    void ApplyGravity()
    {
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = 0f;
        }
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    void HandleJump(bool jumpInput)
    {
        if (jumpInput && isGrounded)
        {
            velocity.y += Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }

    bool IsGrounded()
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
