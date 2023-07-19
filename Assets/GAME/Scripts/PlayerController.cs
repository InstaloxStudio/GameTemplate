using System.Collections;
using System.Collections.Generic;
using UnityEngine;


///summary
/// this is the player controller class that handles input and possession of pawns, the gamemode spawns this in
/// this class is used to control pawns using possess
///summary
public class PlayerController : MonoBehaviour
{
    public Pawn activeCharacter;
    private Vector2 movementInput;
    private Vector2 rotationInput;
    private bool jumpInput;

    public Vector2 MovementInput { get { return movementInput; } }
    public Vector2 RotationInput { get { return rotationInput; } }
    public bool JumpInput { get { return jumpInput; } }

    [SerializeField]
    private Camera activeCamera;

    public Camera ActiveCamera { get { return activeCamera; } }

    protected CameraController cameraController;
    public CameraController CameraController { get { return cameraController; } }
    public Pawn GetCharacter => activeCharacter;
    public List<IControllable> controlledObjects = new List<IControllable>();

    //fields for raycasting to the mouse
    private Ray ray;
    private RaycastHit mouseHit;
    private Vector3 mousePosition;

    public Vector3 MousePosition => mousePosition;
    public RaycastHit MouseHit => mouseHit;

    private bool isMouseLocked = false;
    private bool isMouseVisible = true;

    public bool IsMouseLocked => isMouseLocked;
    public bool IsMouseVisible => isMouseVisible;


    void Update()
    {

        //send all input to the active character
        movementInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        rotationInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        jumpInput = Input.GetButtonDown("Jump");

        //raycast to the mouse position to get the world position
        ray = activeCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out mouseHit))
        {
            mousePosition = mouseHit.point;
        }

        foreach (IControllable controlledObject in controlledObjects)
        {
            controlledObject.ReceiveInput(movementInput, rotationInput, jumpInput);
        }

        if (cameraController != null)
        {
            cameraController.HandleCameraUpdate();
        }
    }

    public void Possess(Pawn pawn)
    {
        if (activeCharacter != null)
        {
            Unpossess();
        }

        activeCharacter = pawn;

        cameraController = activeCharacter.CameraController;
        if (cameraController != null)
        {
            activeCamera = cameraController.Camera;
            activeCamera.enabled = true;
            cameraController.Target = pawn.cameraTarget;

        }
        else
        {
            Debug.LogError("CameraController is missing on the activeCharacter GameObject");
        }
        activeCharacter.Possessed(this);

    }

    public void Unpossess()
    {
        if (activeCharacter != null)
        {
            activeCharacter.Unpossessed(this);
        }

        activeCharacter = null;
        cameraController = null;
        activeCamera = null;
    }


    public Pawn GetActiveCharacter()
    {
        return activeCharacter;
    }

    public void RegisterControlledObject(IControllable controlledObject)
    {
        if (!controlledObjects.Contains(controlledObject))
            controlledObjects.Add(controlledObject);
    }

    public void UnregisterControlledObject(IControllable controlledObject)
    {
        if (controlledObjects.Contains(controlledObject))
            controlledObjects.Remove(controlledObject);
    }

    public void LockMouse()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        isMouseLocked = true;
        isMouseVisible = false;
    }

    public void UnlockMouse()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        isMouseLocked = false;
        isMouseVisible = true;
    }

    public void ToggleMouseLock()
    {
        if (isMouseLocked)
        {
            UnlockMouse();
        }
        else
        {
            LockMouse();
        }
    }

    public void ToggleMouseVisibility()
    {
        if (isMouseVisible)
        {
            Cursor.visible = false;
            isMouseVisible = false;
        }
        else
        {
            Cursor.visible = true;
            isMouseVisible = true;
        }
    }

    public void ToggleMouseLockAndVisibility()
    {
        if (isMouseLocked)
        {
            UnlockMouse();
            ToggleMouseVisibility();
        }
        else
        {
            LockMouse();
            ToggleMouseVisibility();
        }
    }

    public void SetMouseLock(bool isLocked)
    {
        if (isLocked)
        {
            LockMouse();
        }
        else
        {
            UnlockMouse();
        }
    }
}


