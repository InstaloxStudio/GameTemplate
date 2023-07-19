using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    void Update()
    {

        //send all input to the active character
        movementInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        rotationInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        jumpInput = Input.GetButtonDown("Jump");

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
            activeCharacter.Unpossessed(this);
        }
        if (cameraController != null)
        {
            cameraController = null;
        }

        if (activeCamera != null)
        {
            activeCamera = null;
        }

        activeCharacter = pawn;

        cameraController = activeCharacter.CameraController;
        if (cameraController != null)
        {
            activeCamera = cameraController.Camera;
            activeCamera.enabled = true;
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
}


