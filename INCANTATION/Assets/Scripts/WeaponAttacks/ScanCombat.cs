using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScanCombat : WeaponAttack
{
    public float attackRange = 1.1f;
    public float offset;

    private void Start()
    {
        InitializeWeapon();
    }

    // Update is called once per frame
    void Update()
    {
        if(canAttack() && Input.GetButton("Fire1"))
        {
            ScanAttack();
            pcm.PlayerAttackAnimation();
            AttackDelay();
        }
    }

    private void ScanAttack()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
        foreach(Collider2D collision in colliders)
        {
            if(collision.TryGetComponent(out EnemyStats enemy))
            {
                enemy.TakeDamage(playerStats.damage.GetValue());
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
