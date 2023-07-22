using UnityEngine;

public class HealthComponent : MonoBehaviour
{
    public float maxHealth = 100f;
    public float health = 100f;

    public event System.Action<Pawn> OnDeath;
    public float HealthPercentage
    {
        get
        {
            return health / maxHealth;
        }
    }

    public float Health { get => health; set => health = value; }

    private void Start()
    {
        health = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            (FPSGameMode.Instance as FPSGameMode).OnPawnKilled(this.GetComponent<Pawn>());
            Die();
        }
    }

    public void TakeDamage(DamageSource source)
    {
        health -= source.damageAmount;
        if (health <= 0)
        {
            (FPSGameMode.Instance as FPSGameMode).OnPawnKilled(this.GetComponent<Pawn>());
            Die();
        }
    }

    void Die()
    {
        OnDeath?.Invoke(this.GetComponent<Pawn>());
        Destroy(gameObject);
    }
}