using UnityEngine;

public class BulletCasing : MonoBehaviour
{
    public float ejectionSpeed = 10f;
    public float lifeTime = 2f;

    void Start()
    {
        var vel = (transform.right * ejectionSpeed) + Vector3.up;

        //add velocity to the object
        GetComponent<Rigidbody>().velocity = vel;
        // Destroy the bullet after 2 seconds
        Destroy(gameObject, lifeTime);
    }
}