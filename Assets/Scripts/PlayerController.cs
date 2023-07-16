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

    public List<IControllable> controlledObjects = new List<IControllable>();

    void Update()
    {
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

        //if (activeCharacter != null)
        //{
        // activeCharacter.ReceiveInput(movementInput, rotationInput, jumpInput);
        //}
    }

    public void PossessCharacter(Pawn newCharacter)
    {
        if (cameraController != null)
        {
            cameraController.enabled = false;
            //disable the camera in the cameracontroller
        }

        if (activeCamera != null)
        {
            activeCamera.gameObject.SetActive(false);
        }

        activeCharacter = newCharacter;
        cameraController = activeCharacter.GetComponentInChildren<CameraController>();
        if (cameraController != null)
        {
            cameraController.enabled = true;

            //activate the camera in the cameracontroller
            activeCamera = cameraController.Cam;
            activeCamera.enabled = true;
        }
        else
        {
            Debug.LogError("CameraController is missing on the activeCharacter GameObject");
        }

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


