using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    //change to a constant for no chargingVVV
    [HideInInspector] public float projectileVelocity;
    private Rigidbody2D rb;

    private int damage = 0;

    //should be awake??
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        //VVchange to be deleted on impact or duration
        Destroy(gameObject, 4f);
    }

    public void SetDamage(int newDamage)
    {
        damage = newDamage;
    }

    private void FixedUpdate()
    {
        rb.velocity = transform.up * projectileVelocity;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.TryGetComponent(out EnemyStats enemy))
        {
            enemy.TakeDamage(damage);
        }

        if (collision.gameObject.CompareTag("Player"))
        {
            projectileVelocity *= 3f;
            return;
        }
        //make it do that they dont disappear with collision on player

        //projectile death animation
        Destroy(gameObject);
    }
}
