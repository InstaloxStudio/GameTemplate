using UnityEngine;

public class UFOPawn : Pawn
{
    [Header("Movement")]
    public float speed = 10f;
    public float turnSpeed = 60f;
    public float brakeForce = 20f;
    public float brakeDrag = 2f; // Added drag for braking
    public float normalDrag = 0f; // Normal drag when moving

    [Header("Rotation")]
    public bool lockRotation = false;
    [Header("Camera Controllers")]
    public CameraController defaultCameraController;
    public CameraController tractorBeamCameraController;
    public UFOViewState currentView = UFOViewState.Default;


    [Header("Tractor Beam")]
    public TractorBeam tractorBeam;
    private bool jumpInput;

    private Rigidbody rb;
    private Vector2 movementInput;
    private Vector2 rotationInput;
    public override void Awake()
    {
        base.Awake();
        rb = GetComponent<Rigidbody>();
        rb.drag = normalDrag; // Set normal drag on awake
        tractorBeam = GetComponentInChildren<TractorBeam>();
    }

    public override void ReceiveInput(Vector2 movementInput, Vector2 rotationInput, bool jumpInput)
    {
        this.movementInput = movementInput;
        this.rotationInput = rotationInput;
        this.jumpInput = jumpInput;
    }

    private void Update()
    {
        var direction = CalculateDirection(movementInput);
        if (Input.GetMouseButtonDown(1))
        {
            SwapView();
        }
        HandleUFO(direction);
        RotateCharacter(direction, movementInput);
        tractorBeam.HandleInput();
    }
    public void HandleUFO(Vector3 direction)
    {

        if (movementInput == Vector2.zero)
        {
            rb.velocity = Vector3.zero; // UFO comes to an instant stop when there is no input.
        }
        else
        {
            // UFO always moves in the direction it is facing.
            Vector3 moveDirection = transform.forward * movementInput.y + transform.right * movementInput.x;
            rb.velocity = moveDirection * speed;
        }
    }

    public virtual void RotateCharacter(Vector3 direction, Vector2 movementInput)
    {
        if (lockRotation)
        {
            Quaternion targetRotation = Quaternion.Euler(this.PlayerController.PossessedCamera.transform.eulerAngles.x,
                                                          this.PlayerController.PossessedCamera.transform.eulerAngles.y,
                                                          0);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * turnSpeed);
            return;
        }

        if (movementInput != Vector2.zero)
        {
            Quaternion targetRotation = Quaternion.Euler(this.PlayerController.PossessedCamera.transform.eulerAngles.x,
                                                          this.PlayerController.PossessedCamera.transform.eulerAngles.y,
                                                          0);
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
        Vector3 brakeForceVector = -rb.velocity.normalized * brakeForce;
        rb.AddForce(brakeForceVector, ForceMode.Acceleration);
    }

    public void SwitchView(UFOViewState state)
    {
        switch (state)
        {
            case UFOViewState.Default:
                PlayerController.ChangeCameraController(defaultCameraController);
                currentView = UFOViewState.Default;
                break;
            case UFOViewState.TractorBeam:
                PlayerController.ChangeCameraController(tractorBeamCameraController);
                currentView = UFOViewState.TractorBeam;
                break;
        }
    }

    public void SwapView()
    {
        if (currentView == UFOViewState.Default)
        {
            SwitchView(UFOViewState.TractorBeam);
        }
        else
        {
            SwitchView(UFOViewState.Default);
        }
    }
}

public enum UFOViewState
{
    Default,
    TractorBeam
}
