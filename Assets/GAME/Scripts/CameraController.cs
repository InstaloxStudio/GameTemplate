using UnityEngine;

/// <summary>
/// Controller for camera attached to pawns
/// </summary>
public abstract class CameraController : MonoBehaviour, IControllable
{
    protected Transform target; // The target that the camera should follow (usually the player)
    [SerializeField]
    private Camera cam; // The camera that is being controlled
    public Transform Target { get { return target; } set { target = value; } }
    public Camera Camera { get { return cam; } set { cam = value; } }

    public Pawn ControlledPawn { get { return GameMode.Instance.GetPlayerController().GetCharacter; } }
    public bool IsPossessed { get; set; }


    protected virtual void Start()
    {
        PlayerController playerController = GameMode.Instance.GetPlayerController();
        if (playerController != null)
        {
            target = playerController.GetActiveCharacter().cameraTarget;
            //register 
            playerController.RegisterControlledObject(this);

        }
    }

    protected virtual void Update()
    {
        if (IsPossessed)
            HandleCameraUpdate();
    }

    public abstract void HandleCameraUpdate();
    public abstract void ReceiveInput(Vector2 movementInput, Vector2 rotationInput, bool jumpInput);

    public virtual void OnDestroy()
    {
        PlayerController playerController = GameMode.Instance.GetPlayerController();
        if (playerController != null)
        {
            playerController.UnregisterControlledObject(this);
        }
    }


}
