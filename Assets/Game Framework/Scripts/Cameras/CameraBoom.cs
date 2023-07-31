using UnityEngine;

public class CameraBoom : MonoBehaviour
{
    public float distance = 5.0f;  // Distance from the camera boom to the camera
    public Vector3 offset = Vector3.zero;  // Offset from the camera boom to the camera

    public Vector3 CameraPosition;

    [SerializeField]
    private Camera cam;  // The camera attached to the camera boom

    public Camera Camera => cam;


    void Start()
    {
        cam = GetComponentInChildren<Camera>();  // Assume the camera is a child of the camera boom
        if (cam == null)
        {
            Debug.LogError("Camera component is missing on the child of the CameraBoom GameObject");
        }
    }

    void Update()
    {
        if (cam != null)
        {
            // Set the camera's position to be offset from the camera boom
            CameraPosition = transform.position - transform.forward * distance + offset;
            cam.transform.position = CameraPosition;
        }
    }

    void OnDrawGizmos()
    {
        if (cam != null)
        {
            // Set the camera's position to be offset from the camera boom
            CameraPosition = transform.position - transform.forward * distance + offset;
            cam.transform.position = CameraPosition;
        }
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position - transform.forward * distance + offset);
    }
}
