using UnityEngine;

public class UFOPawn : Pawn
{
    public float speed = 10f;
    public float turnSpeed = 60f;
    public float hoverHeight = 5.0f;
    public float hoverForce = 10.0f;
    public float hoverDamping = 0.5f;
    public float brakeForce = 20f;
    public float brakeDrag = 2f; // Added drag for braking
    public float normalDrag = 0f; // Normal drag when moving

    private Rigidbody rb;

    private Vector2 movementInput;
    private Vector2 rotationInput;
    private bool jumpInput;
    public bool lockRotation = false;
    public override void Awake()
    {
        base.Awake();
        // Get the Rigidbody component.
        rb = GetComponent<Rigidbody>();
        rb.drag = normalDrag; // Set normal drag on awake
    }

    public override void ReceiveInput(Vector2 movementInput, Vector2 rotationInput, bool jumpInput)
    {
        // Store the inputs.
        this.movementInput = movementInput;
        this.rotationInput = rotationInput;
        this.jumpInput = jumpInput;
    }

    private void Update()
    {
        var direction = CalculateDirection(movementInput);
        RaycastHit hit;


        if (Physics.Raycast(transform.position, -Vector3.up, out hit, hoverHeight))
        {
            // We're close to "something" below us - apply an upwards force.
            float proportionalHeight = (hoverHeight - hit.distance) / hoverHeight;

            // Apply an upwards force proportional to the calculated height, reduced by damping.
            Vector3 appliedHoverForce = Vector3.up * proportionalHeight * hoverForce;

            // Applying damping to the force when the UFO is moving downwards
            if (rb.velocity.y < 0)
                appliedHoverForce *= hoverDamping;

            rb.AddForceAtPosition(appliedHoverForce, hit.point, ForceMode.Acceleration);
        }
        else
        {
            // We're far away from anything - stop floating.
            //rb.AddForce(Vector3.down * hoverForce, ForceMode.Acceleration);
        }

        // Only apply braking force if no movement input is given.
        if (movementInput == Vector2.zero)
        {
            ApplyBrakes();
            rb.drag = brakeDrag; // Set brake drag
        }
        else
        {
            rb.drag = normalDrag; // Set normal drag
        }

        rb.AddForce(direction * speed * Time.deltaTime, ForceMode.VelocityChange);
        RotateCharacter(direction, movementInput);
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

    public void ApplyBrakes()
    {
        // Apply an opposing force proportional to the current speed.
        Vector3 brakeForceVector = -rb.velocity.normalized * brakeForce;
        rb.AddForce(brakeForceVector, ForceMode.Acceleration);
    }

}
