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

    [ReadOnly] public bool isDead;

    private void Awake()
    {
        damage.SetBaseValue(baseDamage);
        armor.SetBaseValue(baseArmor);
        attackSpeed.SetBaseValue(baseAttackSpeed);
        movementSpeed.SetBaseValue(baseMovementSpeed);
    }

    public override void TakeDamage(int damage)
    {
        if (isDead)
        {
            return;
        }

        base.TakeDamage(damage);
    }

    public override void Die()
    {
        //VVV remove later
        base.Die();
        gameObject.GetComponent<Collider2D>().enabled = false;
        isDead = true;

        //death animation
        //loot

        //VVV change later to instant
        Destroy(gameObject, 1.5f);
    }
}
