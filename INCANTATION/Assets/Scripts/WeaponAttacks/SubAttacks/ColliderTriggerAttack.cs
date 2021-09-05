using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderTriggerAttack : MonoBehaviour
{
    private PlayerStats playerStats;

    private void Start()
    {
        playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.TryGetComponent(out EnemyStats enemy))
        {
            enemy.TakeDamage(playerStats.damage.GetValue());
        }
    }
}
