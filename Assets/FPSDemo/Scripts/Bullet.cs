using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage = 10f;
    public float speed = 10f;
    public float lifeTime = 10f;
    public int bounces = 2;


    public void OnCollisionEnter(Collision collision)
    {
        Damageable damageable = collision.collider.GetComponent<Damageable>();
        if (damageable != null)
        {
            DamageSource damageSource = new DamageSource(damage, DamageType.Generic);
            damageable.TakeDamage(damageSource);
            Destroy(gameObject);
        }
        else
        {
            //if we hit something that isn't damageable, bounce
            if (bounces > 0)
            {
                bounces--;

            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

    void Start()
    {
        //add velocity to the bullet
        GetComponent<Rigidbody>().velocity = transform.forward * speed;

        // Destroy the bullet after 2 seconds
        Destroy(gameObject, lifeTime);
    }
}