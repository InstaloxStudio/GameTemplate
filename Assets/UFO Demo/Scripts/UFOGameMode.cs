using UnityEngine;

public class UFOGameMode : GameMode
{
    public override void Awake()
    {
        base.Awake();
        //set gravity
        Physics.gravity = new Vector3(0, -9.81f, 0);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GetPlayerController().ToggleMouseLock();
        }
    }
}