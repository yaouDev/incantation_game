using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadHitbox : MonoBehaviour
{
    public CharacterStats stats;

    public int damageMultiplierBase = 9;
    private int damageMultiplier;

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

        if (collision.gameObject.CompareTag("PlayerProjectile"))
        {
            stats.TakeDamage(collision.gameObject.GetComponent<Projectile>().damage * damageMultiplier);
        }
    }
}
