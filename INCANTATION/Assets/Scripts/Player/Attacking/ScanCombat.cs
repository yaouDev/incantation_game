using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScanCombat : WeaponAttack
{
    private float attackRange;

    // Start is called before the first frame update
    void Start()
    {
        attackRange = pcm.attackRange;
    }

    // Update is called once per frame
    void Update()
    {
        //VV currently no attackspeed
        if(canAttack && Input.GetButton("Fire1"))
        {
            ScanAttack();
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
