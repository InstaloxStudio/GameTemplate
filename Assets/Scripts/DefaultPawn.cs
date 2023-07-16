using UnityEngine;

public class DefaultPawn : Pawn
{
    public float speed = 10.0f;
    public float turnSpeed = 100.0f;

    private Vector3 velocity;
    private Vector3 rotation;

    public override void ReceiveInput(Vector2 movementInput, Vector2 rotationInput, bool jumpInput)
    {
        MovePawn(movementInput);
        RotatePawn(rotationInput);
    }

    public void MovePawn(Vector2 movementInput)
    {
        Vector3 direction = new Vector3(movementInput.x, 0, movementInput.y);
        direction.Normalize();

        transform.Translate(direction * speed * Time.deltaTime);
    }

    public void RotatePawn(Vector2 rotationInput)
    {
        rotation.x -= rotationInput.y * turnSpeed * Time.deltaTime;
        rotation.y += rotationInput.x * turnSpeed * Time.deltaTime;

        // Apply rotation
        transform.eulerAngles = rotation;
    }
}
