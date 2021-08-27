using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : CharacterStats
{
    public int baseMaxHealth = 1000;

    public int baseDamage = 2;
    public int baseArmor = 0;
    public int baseAttackSpeed = 10;
    public int baseMovementSpeed = 50;

    private void Awake()
    {
        damage.SetBaseValue(baseDamage);
        armor.SetBaseValue(baseArmor);
        attackSpeed.SetBaseValue(baseAttackSpeed);
        movementSpeed.SetBaseValue(baseMovementSpeed);

        //VVV how tf does it work without this?
        //maxHealth.SetBaseValue(baseMaxHealth);
    }

    public override void Die()
    {
        base.Die();

        //death animation
        //loot

        //VVV change later to instant
        Destroy(gameObject, 1.5f);
    }
}
