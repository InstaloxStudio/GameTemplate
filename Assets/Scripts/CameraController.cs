using UnityEngine;

public abstract class CameraController : MonoBehaviour, IControllable
{
    protected Transform target; // The target that the camera should follow (usually the player)
    [SerializeField]
    protected Camera cam; // The camera that is being controlled
    public Transform Target
    {
        get { return target; }
        set { target = value; }
    }
    public Camera Cam
    {
        get { return cam; }
        set { cam = value; }
    }

    protected virtual void Start()
    {
        PlayerController playerController = GameMode.Instance.GetPlayerController();
        if (playerController != null)
        {
            target = playerController.GetActiveCharacter().cameraTarget;

        }
    }

    protected virtual void Update()
    {

        HandleCameraUpdate();

    }
    // Update the position, rotation, or any other parameters of the camera
    public abstract void HandleCameraUpdate();
    public abstract void ReceiveInput(Vector2 movementInput, Vector2 rotationInput, bool jumpInput);

    public virtual void OnEnable()
    {
        //register as a controllable
        PlayerController playerController = GameMode.Instance.GetPlayerController();
        if (playerController != null)
        {
            playerController.RegisterControlledObject(this);
        }
    }

    public virtual void OnDisable()
    {
        //unregister as a controllable
        PlayerController playerController = GameMode.Instance.GetPlayerController();
        if (playerController != null)
        {
            playerController.UnregisterControlledObject(this);
        }
    }

}
