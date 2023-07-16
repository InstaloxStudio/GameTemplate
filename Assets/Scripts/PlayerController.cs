using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public PlayerCharacter activeCharacter;
    private Vector2 movementInput;
    private Vector2 rotationInput;
    private bool jumpInput;

    public Vector2 MovementInput { get { return movementInput; } }
    public Vector2 RotationInput { get { return rotationInput; } }
    public bool JumpInput { get { return jumpInput; } }

    [SerializeField]
    private Camera activeCamera;

    public Camera ActiveCamera { get { return activeCamera; } }

    void Update()
    {
        movementInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        rotationInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        jumpInput = Input.GetButtonDown("Jump");

        if (activeCharacter != null)
        {
            activeCharacter.ReceiveInput(movementInput, rotationInput, jumpInput);
        }
    }

    public void PossessCharacter(PlayerCharacter newCharacter)
    {
        activeCharacter = newCharacter;
        if (activeCamera != null)
        {
            activeCamera.gameObject.SetActive(false);
        }

        activeCamera = activeCharacter.GetComponentInChildren<Camera>();
        if (activeCamera != null)
        {
            activeCamera.gameObject.SetActive(true);
            ThirdPersonCamera thirdPersonCamera = activeCamera.GetComponent<ThirdPersonCamera>();
            if (thirdPersonCamera != null)
            {
                // thirdPersonCamera.target = activeCharacter.transform;
            }
            else
            {
                Debug.LogError("ThirdPersonCamera component is missing on the activeCamera GameObject");
            }
        }
        else
        {
            Debug.LogError("Camera component is missing on the activeCharacter GameObject");
        }
    }


    public PlayerCharacter GetActiveCharacter()
    {
        return activeCharacter;
    }
}


