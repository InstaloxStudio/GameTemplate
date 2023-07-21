using UnityEngine;

public class Health : MonoBehaviour
{
    public float health = 100f;

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            (FPSGameMode.Instance as FPSGameMode).OnPawnKilled(this.GetComponent<Pawn>());
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }
}