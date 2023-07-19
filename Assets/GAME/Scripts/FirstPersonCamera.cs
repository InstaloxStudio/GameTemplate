using UnityEngine;

public class FirstPersonCamera : CameraController
{
    public float sensitivity = 100f;

    private float verticalRotation = 0f;

    // Clamp the vertical angle of the camera to avoid flipping
    public float verticalAngleLimit = 80f;

    public override void ReceiveInput(Vector2 movementInput, Vector2 rotationInput, bool jumpInput)
    {
        //only run if the pawn is possessed
        //print controlled pawn

        if (IsPossessed)
            HandleRotation(rotationInput);
    }

    private void HandleRotation(Vector2 rotationInput)
    {
        // Adjust the rotation based on the mouse input and the sensitivity setting
        float mouseX = rotationInput.x * sensitivity * Time.deltaTime;
        float mouseY = rotationInput.y * sensitivity * Time.deltaTime;

        // We subtract because y input is inverted (i.e., moving the mouse up gives you a negative value)
        verticalRotation -= mouseY;

        // Clamp the vertical rotation so the player can't flip the camera upside-down
        verticalRotation = Mathf.Clamp(verticalRotation, -verticalAngleLimit, verticalAngleLimit);

        // Apply the rotation
        transform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
        ControlledPawn.transform.Rotate(Vector3.up * mouseX);
    }

    public override void HandleCameraUpdate()
    {

    }
}
