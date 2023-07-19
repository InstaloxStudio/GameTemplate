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

        if (Input.GetKeyDown(KeyCode.Q))
        {
            //find any other pawn and posses it
            foreach (var pawn in GameMode.Instance.Pawns)
            {
                if (pawn != this)
                {
                    this.GetPlayerController().Possess(pawn);
                    break;
                }
            }
        }
    }
}
