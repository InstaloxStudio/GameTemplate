using System.Collections;
using System.Collections.Generic;
using UnityEngine;


///summary
/// this is the player controller class that handles input and possession of pawns, the gamemode spawns this in
/// this class is used to control pawns using possess
///summary
public class PlayerController : MonoBehaviour
{
    [Header("Input")]
    public Pawn possessedPawn;
    private Vector2 movementInput;
    private Vector2 rotationInput;
    private bool jumpInput;

    public Vector2 MovementInput { get { return movementInput; } }
    public Vector2 RotationInput { get { return rotationInput; } }
    public bool JumpInput { get { return jumpInput; } }

    [Header("Camera")]
    [SerializeField]
    private Camera possessedCamera;
    public Camera PossessedCamera { get { return possessedCamera; } }
    protected CameraController cameraController;
    public CameraController CameraController { get { return cameraController; } }

    [Header("Controlled Objects")]
    public List<IControllable> controlledObjects = new List<IControllable>();
    public Pawn GetCharacter => possessedPawn;

    [Header("Raycasting")]
    //fields for raycasting to the mouse
    private Ray ray;
    private RaycastHit mouseHit;
    private Vector3 mousePosition;

    public Vector3 MousePosition => mousePosition;
    public RaycastHit MouseHit => mouseHit;

    [Header("Cursor Settings")]
    [SerializeField]
    private bool isMouseLocked = false;
    [SerializeField]
    private bool isMouseVisible = true;

    public bool IsMouseLocked => isMouseLocked;
    public bool IsMouseVisible => isMouseVisible;

    [Header("Misc")]
    private bool hasInput = true;
    public bool HasInput => hasInput;

    void Update()
    {
        if (!hasInput)
            return;

        if (possessedPawn == null)
        {
            return;
        }

        if (possessedCamera == null)
        {
            return;
        }

        //send all input to the active character
        movementInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        rotationInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        jumpInput = Input.GetButtonDown("Jump");

        //raycast to the mouse position to get the world position
        ray = possessedCamera.ScreenPointToRay(Input.mousePosition);
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

        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (GameMode.Instance.Pawns.Count <= 0)
                return;
            //grab a random pawn and posses it
            int randomIndex = Random.Range(0, GameMode.Instance.Pawns.Count);
            if (GameMode.Instance.Pawns[randomIndex] != possessedPawn)
                Possess(GameMode.Instance.Pawns[randomIndex]);
        }
    }

    public void Possess(Pawn pawn)
    {
        if (possessedPawn != null)
        {
            Unpossess();
        }

        possessedPawn = pawn;

        cameraController = possessedPawn.CameraController;
        if (cameraController != null)
        {
            possessedCamera = cameraController.Camera;
            possessedCamera.enabled = true;
            cameraController.Target = pawn.cameraTarget;

        }
        else
        {
            Debug.LogError("CameraController is missing on the activeCharacter GameObject");
        }
        possessedPawn.Possessed(this);

    }

    public void Unpossess()
    {
        if (possessedPawn != null)
        {
            possessedPawn.Unpossessed(this);
        }

        possessedPawn = null;
        cameraController = null;
        possessedCamera = null;
    }


    public Pawn GetActiveCharacter()
    {
        return possessedPawn;
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

    public virtual void DisableInput()
    {
        hasInput = false;
    }

    public virtual void EnableInput()
    {
        hasInput = true;
    }
}


