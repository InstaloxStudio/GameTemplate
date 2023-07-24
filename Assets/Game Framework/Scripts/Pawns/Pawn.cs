using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

/// <summary>
/// Base class for all pawns, which are objects that can be possessed by a controller.
/// </summary>
public class Pawn : MonoBehaviour, IControllable
{

    //event for when the pawn is possessed
    public event Action<PlayerController> OnPossessed;
    public event Action<PlayerController> OnUnpossessed;

    public event Action<Pawn> OnDeathEvent;
    public Transform cameraTarget;

    private CameraController cameraController;
    public CameraController CameraController { get { return cameraController; } }
    public PlayerController PlayerController { get { return GameMode.Instance.GetPlayerController(); } }

    public bool DestroyOnDeath = true;

    public virtual void ReceiveInput(Vector2 movementInput, Vector2 rotationInput, bool jumpInput)
    {
        // This method should be overridden by the subclasses to define the behavior
    }
    public bool isPossessed = false;
    public void Awake()
    {
        cameraController = GetComponentInChildren<CameraController>();

    }

    public virtual void Start()
    {
        this.PlayerController.RegisterControlledObject(this);
        if (!isPossessed)
            DisableCameraController();

        this.Initialize();
    }

    public virtual void DisableCameraController()
    {
        if (cameraController != null)
        {
            cameraController.enabled = false;
            isPossessed = false;
            //disable the camera
            if (cameraController.Camera != null)
            {
                cameraController.Camera.enabled = false;
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
            if (cameraController.Camera != null)
            {
                cameraController.Camera.enabled = true;
                cameraController.GetComponent<AudioListener>().enabled = true;
            }
        }
    }

    protected virtual void OnDestroy()
    {
        // Automatically unregister from the PlayerController when this object is destroyed

        this.PlayerController.UnregisterControlledObject(this);

    }

    public PlayerController GetPlayerController()
    {
        return GameMode.Instance.GetPlayerController();
    }

    // This method is called by the PlayerController when this pawn is possessed
    public virtual void Possessed(PlayerController playerController)
    {
        EnableCameraController();
        OnPossessed?.Invoke(playerController);
        isPossessed = true;
        cameraController.IsPossessed = true;
    }

    // This method is called by the PlayerController when this pawn is unpossessed
    public virtual void Unpossessed(PlayerController playerController)
    {
        DisableCameraController();
        OnUnpossessed?.Invoke(playerController);
        isPossessed = false;
        cameraController.IsPossessed = false;
    }

    public virtual void OnDeath()
    {
        OnDeathEvent?.Invoke(this);
        if (DestroyOnDeath)
            Destroy(gameObject);
    }

    public virtual void Initialize()
    {
        // This method should be overridden by the subclasses to define the behavior
    }
}
