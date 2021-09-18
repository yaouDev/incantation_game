using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderTriggerAttack : MonoBehaviour
{
    private PlayerStats playerStats;
    public bool canDestroyProjectiles;
    public bool isFire;

    private void Start()
    {
        playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.TryGetComponent(out EnemyStats enemy))
        {
            enemy.TakeDamage(playerStats.damage.GetValue());

            if (isFire)
            {
                enemy.effectState.ApplyEffect(Effect.burn, 2f, playerStats.damage.GetValue() / 10);
            }
        }

        if (collision.gameObject.CompareTag("EnemyProjectile") && canDestroyProjectiles)
        {
            Destroy(collision.gameObject);
        }
    }
}
