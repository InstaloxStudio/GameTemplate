using UnityEngine;

/// <summary>
/// Typical third person camera controller
/// </summary>
public class ThirdPersonCamera : CameraController
{
    public CameraBoom cameraBoom; // The camera boom to rotate
    public float mouseSensitivity = 10f; // Sensitivity of mouse movement
    public float minVerticalAngle = -45f; // The minimum vertical angle
    public float maxVerticalAngle = 45f; // The maximum vertical angle
    public float groundCheckDistance = 0.5f; // The distance of the ground check
    public float xRotationSmoothTime = 0.12f; // The time it takes for the camera to smooth to its rotation
    public float yRotationSmoothTime = 0.01f; // The time it takes for the camera to smooth to its rotation
    private float currentX = 0f; // Current rotation around the Y-axis
    private float currentY = 0f; // Current rotation up and down (tilt)
    private Vector2 rotationInput;
    // For smoothing
    private float yRotationV;
    private float xRotationV;
    [SerializeField]
    private PlayerController playerController;


    public override void HandleCameraUpdate()
    {


        float targetX = currentX + rotationInput.x * mouseSensitivity;
        float targetY = currentY - rotationInput.y * mouseSensitivity;

        // Clamp the vertical angle to prevent flipping the camera upside down
        targetY = Mathf.Clamp(targetY, minVerticalAngle, maxVerticalAngle);


        // Check if the new camera position would be below the ground
        if (Physics.Raycast(cameraBoom.CameraPosition, -Vector3.up, groundCheckDistance))
        {
            if (targetY < minVerticalAngle)
            {
                targetY = minVerticalAngle;
            }
        }

        currentX = Mathf.SmoothDamp(currentX, targetX, ref xRotationV, xRotationSmoothTime);
        currentY = Mathf.SmoothDamp(currentY, targetY, ref yRotationV, yRotationSmoothTime);
        cameraBoom.transform.rotation = Quaternion.Euler(currentY, currentX, 0);
    }

    public override void ReceiveInput(Vector2 movementInput, Vector2 rotationInput, bool jumpInput)
    {
        this.rotationInput = rotationInput;

    }
}
