using System;
using UnityEngine;

public class TractorBeam : MonoBehaviour
{
    public float tractorBeamRange = 100f;
    public float tractorBeamForce = 100f;

    public Rigidbody tractorTarget;

    public bool isBeamOn = false;
    public bool isLockedOn = false;

    public bool isLightOn = false;
    public Light tractorBeamLight;

    public void ToggleLight()
    {
        isLightOn = !isLightOn;
        tractorBeamLight.enabled = isLightOn;
    }

    public void SetTarget(Transform target)
    {
        tractorTarget = target.GetComponent<Rigidbody>();
    }

    public void ClearTarget()
    {
        tractorTarget = null;
    }

    public void LockOnTarget()
    {
        if (!tractorTarget)
        {
            return;
        }
        //point the beam at the target
        transform.LookAt(tractorTarget.transform.position);

    }

    public void Update()
    {
        if (isBeamOn)
        {
            if (tractorTarget)
            {
                PullTarget();
            }
            else
            {
                CheckForBeamTargets();
            }
        }
        else
        {
            tractorTarget = null;
        }

        if (isLockedOn)
        {
            LockOnTarget();
        }
    }

    private void CheckForBeamTargets()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, tractorBeamRange))
        {
            tractorTarget = hit.rigidbody;
        }
    }

    private void PullTarget()
    {
        // Apply an upwards force to the target
        Vector3 force = (transform.position - tractorTarget.transform.position) * tractorBeamForce;
        tractorTarget.AddForce(force);

        // Apply a drag to the target
        tractorTarget.drag = 10f;
        tractorTarget.angularDrag = 10f;
        tractorTarget.transform.rotation = Quaternion.Slerp(tractorTarget.transform.rotation, transform.rotation, Time.deltaTime * 10f);

    }

    private void ReleaseTarget()
    {
        tractorTarget.drag = 0f;
        tractorTarget.angularDrag = 0.05f;
        tractorTarget = null;
    }

    public void HandleInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isBeamOn = true;
        }
        if (Input.GetMouseButtonUp(0))
        {
            isBeamOn = false;
            if (tractorTarget)
            {
                ReleaseTarget();
            }
        }

    }
}