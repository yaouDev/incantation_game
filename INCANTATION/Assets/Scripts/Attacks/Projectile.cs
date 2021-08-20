using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    //change to a constant for no chargingVVV
    [HideInInspector] public float projectileVelocity;
    private Rigidbody2D rb;
    private SpriteRenderer sr;

    private int damage = 0;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
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

        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("PlayerProjectile"))
        {
            Physics2D.IgnoreCollision(collision.collider, GetComponent<CapsuleCollider2D>());
            return;
        }

        //projectile death animation
        Destroy(gameObject);
    }

    public SpriteRenderer GetSpriteRenderer()
    {
        return sr;
    }
}
