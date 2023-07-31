using UnityEngine;

public class OrbitCamera : CameraController
{
    public float sensitivity = 100f;
    private float xRotation = 0f;
    private float yRotation = 0f;

    // Clamp the vertical angle of the camera to avoid flipping
    public float verticalAngleLimitMin = -80f;
    public float verticalAngleLimitMax = 80f;

    public override void ReceiveInput(Vector2 movementInput, Vector2 rotationInput, bool jumpInput)
    {
        if (IsPossessed)
        {
            HandleRotation(rotationInput);
        }
    }

    private void HandleRotation(Vector2 rotationInput)
    {
        // Adjust the rotation based on the mouse input and the sensitivity setting
        float mouseX = rotationInput.x * sensitivity * Time.deltaTime;
        float mouseY = rotationInput.y * sensitivity * Time.deltaTime;

        // We subtract because y input is inverted (i.e., moving the mouse up gives you a negative value)
        xRotation -= mouseY;
        yRotation += mouseX;

        // Clamp the vertical rotation so the camera can't flip upside-down
        xRotation = Mathf.Clamp(xRotation, verticalAngleLimitMin, verticalAngleLimitMax);

        // Apply the rotation
        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
    }

    public override void HandleCameraUpdate()
    {

    }
}
