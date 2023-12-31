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

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            this.GetPlayerController().SetMouseLock(true);
            this.GetPlayerController().LockMouse();
        }

        if (Input.GetMouseButtonDown(0))
        {
            var hit = this.GetPlayerController().MouseHit;
            if (hit.collider != null)
            {
                //spawn a cube at hit position
                GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                cube.transform.position = hit.point;

            }
        }
    }
}
