using UnityEngine;

public class Damageable : MonoBehaviour
{
    public delegate void DamageEvent(DamageSource source);

    // Optional events for notifying other components or systems when damage is received or the object dies
    public event DamageEvent OnDamageReceived;
    public event DamageEvent OnDeath;


    private void Start()
    {
        HealthComponent healthComponent = GetComponent<HealthComponent>();
        if (healthComponent != null)
        {
            //subscribe the health component to the damage event
            OnDamageReceived += healthComponent.TakeDamage;
        }
    }

    public void TakeDamage(DamageSource source)
    {
        // Call the damage event
        OnDamageReceived?.Invoke(source);
    }

    private void Die(DamageSource source)
    {
        // Call the death event
        OnDeath?.Invoke(source);

        // Here we could handle game-specific logic, like removing the entity, playing a death animation, etc.
        // For this simple example, we'll just destroy the game object
        Destroy(gameObject);
    }
}
