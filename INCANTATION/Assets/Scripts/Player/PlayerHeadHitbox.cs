using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHeadHitbox : MonoBehaviour
{
    public PlayerStats stats;

    public int damageMultiplierBase = 9;
    private int damageMultiplier;
    /*
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(stats.armor.GetValue() > 0)
        {
            damageMultiplier = (int)(damageMultiplierBase / (stats.armor.GetValue() / 10));
        }
        else
        {
            damageMultiplier = damageMultiplierBase;
        }

        if (collision.gameObject.CompareTag("EnemyProjectile"))
        {
            stats.TakeDamage(collision.gameObject.GetComponent<EnemyProjectile>().damage * damageMultiplier);
        }
    }*/

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (stats.armor.GetValue() > 0)
        {
            damageMultiplier = (int)(damageMultiplierBase / (stats.armor.GetValue() / 10));
        }
        else
        {
            damageMultiplier = damageMultiplierBase;
        }

        if (collision.gameObject.CompareTag("EnemyProjectile"))
        {
            stats.TakeDamage(collision.gameObject.GetComponent<EnemyProjectile>().damage * damageMultiplier);
        }
    }
}
