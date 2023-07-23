using UnityEngine;

public class BillboardComponent : MonoBehaviour
{
    private Camera cam;

    void Update()
    {
        //face the camera
        if (cam == null)
        {
            cam = FindObjectOfType<Camera>();
            //face camera
            //billboard to camera
            transform.forward = cam.transform.forward;

        }
    }
}