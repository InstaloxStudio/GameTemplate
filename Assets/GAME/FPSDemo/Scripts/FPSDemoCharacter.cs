using UnityEngine;

public class FPSDemoCharacter : Character
{
    public Gun gun; // assign this in the inspector

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
            // Instead of spawning a cube, we shoot our gun
            gun.Shoot();
        }
    }
}
