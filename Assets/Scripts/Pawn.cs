using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

/// <summary>
/// Base class for all pawns, which are objects that can be possessed by the player controller
/// </summary>
public class Pawn : MonoBehaviour, IControllable
{

    //event for when the pawn is possessed
    public Action<PlayerController> OnPossessed;
    public Action<PlayerController> OnUnpossessed;

    public Transform cameraTarget;

    private CameraController cameraController;
    public CameraController CameraController { get { return cameraController; } }

    public virtual void ReceiveInput(Vector2 movementInput, Vector2 rotationInput, bool jumpInput)
    {
        // This method should be overridden by the subclasses to define the behavior
    }

    protected virtual void Awake()
    {
        cameraController = GetComponentInChildren<CameraController>();
        DisableCameraController();

        // Automatically register with the PlayerController when this object becomes active
        PlayerController playerController = GameMode.Instance.GetPlayerController();
        if (playerController != null)
        {
            playerController.RegisterControlledObject(this);
        }
    }

    public virtual void DisableCameraController()
    {
        if (cameraController != null)
        {
            cameraController.enabled = false;
            //disable the camera
            if (cameraController.Cam != null)
            {
                cameraController.Cam.enabled = false;
                cameraController.GetComponent<AudioListener>().enabled = false;
            }
        }
    }

    public virtual void EnableCameraController()
    {
        if (cameraController != null)
        {
            cameraController.enabled = true;
            //enable the camera
            if (cameraController.Cam != null)
            {
                cameraController.Cam.enabled = true;
                cameraController.GetComponent<AudioListener>().enabled = true;
            }
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

    // This method is called by the PlayerController when this pawn is possessed
    public virtual void Possessed(PlayerController playerController)
    {
        EnableCameraController();
        if (OnPossessed != null)
        {
            OnPossessed(playerController);
        }
    }

    // This method is called by the PlayerController when this pawn is unpossessed
    public virtual void Unpossessed(PlayerController playerController)
    {
        DisableCameraController();
        if (OnUnpossessed != null)
        {
            OnUnpossessed(playerController);
        }
    }


}
