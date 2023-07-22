using UnityEngine;

public class DamageSource
{
    // Amount of damage this source will cause
    public float damageAmount = 10f;

    public DamageSource(float damageAmount)
    {
        this.damageAmount = damageAmount;
    }

    public DamageSource(float damageAmount, DamageType damageType)
    {
        this.damageAmount = damageAmount;
        this.damageType = damageType;
    }
    // Optional: Types of damage, like fire or electricity. This could be an enum or class if you have many types
    public DamageType damageType = DamageType.Generic;
    public void DealDamage(Damageable target)
    {
        target.TakeDamage(this);
    }
}

public enum DamageType
{
    Generic,    // General or unspecified damage
    Physical,   // Damage from physical causes, like blunt trauma, slashing, or piercing
    Fire,       // Damage from fire or heat
    Ice,        // Damage from cold or freezing
    Electric,   // Damage from electricity or lightning
    Poison,     // Damage from toxins or venom
    Acid,       // Damage from corrosive substances
    Psychic,    // Damage from mental or psychic attacks
    Radiant,    // Damage from holy or radiant energy
    Necrotic,   // Damage from death energy or necrotic magic
    Sonic,      // Damage from sound or vibrations
    Force,      // Damage from magical force energy
    Earth,      // Damage from earth or stone
    Air,        // Damage from air, wind, or gaseous substances
    Water,      // Damage from water or liquids
    Magical     // General damage from magical sources
}
