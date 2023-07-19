using UnityEngine;

/// <summary>
/// Basic first person character pawn with basic movement and jumping.
/// </summary>
public class FirstPersonCharacter : Character
{
    public override void RotateCharacter(Vector3 direction, Vector2 movementInput)
    {
        // Do nothing, the rotation is handled by the camera controller in first person
    }
}
