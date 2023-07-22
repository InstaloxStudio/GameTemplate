using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage = 10f;
    public float speed = 10f;

    public void OnCollisionEnter(Collision collision)
    {
        Damageable damageable = collision.collider.GetComponent<Damageable>();
        if (damageable != null)
        {
            DamageSource damageSource = new DamageSource(damage, DamageType.Generic);
            damageable.TakeDamage(damageSource);
        }
        Destroy(gameObject);
    }

    private void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }



}