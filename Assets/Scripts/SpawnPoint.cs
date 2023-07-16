using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    //draw wireframe sphere in editor
    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, 1);
    }
}