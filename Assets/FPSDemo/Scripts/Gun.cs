using System;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public float damage = 10f;
    public float range = 100f;

    public GameObject bulletPrefab;
    public GameObject bulletCasingPrefab;

    public Transform bulletSpawn;
    public Transform bulletCasingSpawn;

    // This method will "shoot" the gun
    public void Shoot()
    {
        //if bullet prefab is null, just do a raycast
        if (bulletPrefab == null)
        {
            ShootRaycast();
            return;
        }
        else
        {
            ShootBullet();
        }

    }

    private void ShootBullet()
    {
        var bulletCasing = (GameObject)Instantiate(
            bulletCasingPrefab,
            bulletCasingSpawn.position,
            bulletCasingSpawn.rotation);

        // Create the Bullet from the Bullet Prefab
        var bullet = (GameObject)Instantiate(
            bulletPrefab,
            bulletSpawn.position,
            bulletSpawn.rotation);

        Bullet bulletComponent = bullet.GetComponent<Bullet>();
        bulletComponent.damage = damage;
        //make the bullet ignore the player
        Physics.IgnoreCollision(bullet.GetComponent<Collider>(), bulletCasing.GetComponent<Collider>());

        // Add velocity to the bullet
        bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * 6;

        // Destroy the bullet after 2 seconds
        Destroy(bullet, 2.0f);
    }

    private void ShootRaycast()
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
