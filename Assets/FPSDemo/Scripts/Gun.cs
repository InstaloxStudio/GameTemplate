using UnityEngine;

public class Gun : MonoBehaviour
{
    public float damage = 10f;
    public float range = 100f;

    // This method will "shoot" the gun
    public void Shoot()
    {
        // We create a raycast from the gun in the direction it's facing
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, range))
        {
            // If we hit something, we check if it has a damageable component
            Damageable damageable = hit.collider.GetComponent<Damageable>();
            if (damageable != null)
            {
                //create damagesource
                DamageSource damageSource = new DamageSource(damage, DamageType.Generic);


                // If it does, we deal damage to it
                damageable.TakeDamage(damageSource);
            }
        }
    }
}
