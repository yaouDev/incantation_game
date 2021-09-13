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

    public SpriteFlash whiteHit;
    [ReadOnly] public bool isDead;

    private DropLoot loot;

    private void Awake()
    {
        damage.SetBaseValue(baseDamage);
        armor.SetBaseValue(baseArmor);
        attackSpeed.SetBaseValue(baseAttackSpeed);
        movementSpeed.SetBaseValue(baseMovementSpeed);

        loot = GetComponent<DropLoot>();
    }

    public override void TakeDamage(int damage)
    {
        if (isDead)
        {
            return;
        }

        base.TakeDamage(damage);
        whiteHit.Play();
    }

    public override void Die()
    {
        //VVV remove later
        base.Die();
        gameObject.GetComponent<Collider2D>().enabled = false;
        isDead = true;

        //death animation
        if(loot != null)
        {
            Item drop = loot.Drop(gameObject.transform.position);
            Debug.Log("You got a " + drop.rarity + "!" + " " + drop.name);
        }

        //VVV change later to instant
        Destroy(gameObject, 1.5f);
    }
}
