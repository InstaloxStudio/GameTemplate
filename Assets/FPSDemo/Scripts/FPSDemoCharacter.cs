using UnityEngine;

public class FPSDemoCharacter : Character
{
    public Gun gun; // assign this in the inspector
    public PlayerData playerData;
    public HealthComponent healthComponent;

    public Damageable damageable;

    public override void Start()
    {
        base.Start();
        this.DestroyOnDeath = false;
        // Set the player's health to the value in the player data
        this.healthComponent = GetComponent<HealthComponent>();
        playerData.health = this.healthComponent.Health;
        playerData.kills = 0;
        damageable = GetComponent<Damageable>();
    }

    public override void RotateCharacter(Vector3 direction, Vector2 movementInput)
    {
        // Do nothing, the rotation is handled by the camera controller in first person
    }

    void Update()
    {
        //set player data
        playerData.health = this.healthComponent.Health;
        playerData.kills = (FPSGameMode.Instance as FPSGameMode).CurrentKills;

        if (!GetPlayerController().HasInput)
            return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            this.GetPlayerController().SetMouseLock(true);
            this.GetPlayerController().LockMouse();
        }

        if (Input.GetMouseButtonDown(0))
        {
            gun.Shoot();
        }
    }

    override public void OnDeath()
    {
        // Call the base class's OnDeath method
        base.OnDeath();
        var gamemode = GameMode.Instance;
        gamemode.GetPlayerController().DisableInput();
        gamemode.GetPlayerController().SetMouseLock(false);
    }
}
