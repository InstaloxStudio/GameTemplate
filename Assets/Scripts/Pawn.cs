using UnityEngine;
public class Pawn : MonoBehaviour, IControllable
{

    public Transform cameraTarget;
    public virtual void ReceiveInput(Vector2 movementInput, Vector2 rotationInput, bool jumpInput)
    {
        // This method should be overridden by the subclasses to define the behavior
    }

    protected virtual void Awake()
    {
        // Automatically register with the PlayerController when this object becomes active
        PlayerController playerController = GameMode.Instance.GetPlayerController();
        if (playerController != null)
        {
            playerController.RegisterControlledObject(this);
        }
    }

    protected virtual void OnDestroy()
    {
        // Automatically unregister from the PlayerController when this object is destroyed
        PlayerController playerController = GameMode.Instance.GetPlayerController();
        if (playerController != null)
        {
            playerController.UnregisterControlledObject(this);
        }
    }

    protected virtual void OnEnable()
    {
        // Automatically register with the PlayerController when this object becomes active
        PlayerController playerController = GameMode.Instance.GetPlayerController();
        if (playerController != null)
        {
            playerController.RegisterControlledObject(this);
        }
    }

    protected virtual void OnDisable()
    {
        // Automatically unregister from the PlayerController when this object is disabled
        PlayerController playerController = GameMode.Instance.GetPlayerController();
        if (playerController != null)
        {
            playerController.UnregisterControlledObject(this);
        }
    }

    public PlayerController GetPlayerController()
    {
        return GameMode.Instance.GetPlayerController();
    }
}
