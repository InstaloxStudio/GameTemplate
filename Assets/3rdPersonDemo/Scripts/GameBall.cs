using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
public class GameBall : MonoBehaviour
{
    public event Action OnBallDestroyed;

    public bool isActive = true;
    public bool isPickedUp = false;

    public Rigidbody rb;

    public void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void OnDestroy()
    {
        if (OnBallDestroyed != null)
        {
            OnBallDestroyed();
        }
    }


    public void ResetBall()
    {
        isActive = false;
        isPickedUp = false;
        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    public void ActivateBall()
    {
        isActive = true;
    }

    public void DeactivateBall()
    {
        isActive = false;
    }

    public void PickUpBall(Pawn pawn)
    {
        transform.parent = pawn.transform;
        transform.localPosition = transform.forward + transform.up * 2f;
        var rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        rb.useGravity = false;

        isPickedUp = true;
    }

    public void DropBall()
    {
        isPickedUp = false;
        transform.parent = null;
        var rb = GetComponent<Rigidbody>();
        rb.isKinematic = false;
        rb.useGravity = true;

    }

    public void ThrowBall(Vector3 direction, float force)
    {
        rb.AddForce(direction * force, ForceMode.Impulse);
    }

    public void MoveBall(Vector3 direction, float force)
    {
        rb.AddForce(direction * force, ForceMode.Impulse);
    }

    public void Update()
    {
        var ypos = transform.position.y;
        if (ypos < -10f)
        {
            ResetBall();

        }
    }

    //if a pawn collides with the ball, pick it up
    public void OnCollisionEnter(Collision collision)
    {
        var pawn = collision.gameObject.GetComponent<ThirdPersonDemoCharacter>();
        if (pawn != null)
        {
            if (pawn.isPossessed)
            {
                Debug.Log("Pick up ball");
                pawn.PickUpBall(this);
            }
        }

    }

}