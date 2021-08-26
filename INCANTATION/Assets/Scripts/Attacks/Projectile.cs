using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [HideInInspector] public float projectileVelocity;
    [HideInInspector] public float knockbackPower;
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private LayerMask lm;

    private int damage = 0;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        lm = gameObject.layer;

        Physics2D.IgnoreLayerCollision(lm, lm);

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

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (IgnoreCollision(collision))
        {
            return;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.TryGetComponent(out EnemyStats enemy))
        {
            //enemy.TakeDamage(damage);
            if(knockbackPower > 0f)
            {
                enemy.TakeDamage(damage, knockbackPower, transform);
            }
            else
            {
                enemy.TakeDamage(damage);
            }
        }

        if (IgnoreCollision(collision))
        {
            return;
        }

        //projectile death animation
        gameObject.SetActive(false);
        rb.velocity = Vector2.zero;
        Destroy(gameObject, 1f);
    }

    //VVV Excessive?
    private bool IgnoreCollision(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("PlayerProjectile"))
        {
            Physics2D.IgnoreCollision(collision.collider, GetComponent<CapsuleCollider2D>());
            return true;
        }

        if (collision.gameObject.CompareTag("Player"))
        {
            Physics2D.IgnoreCollision(collision.collider, GetComponent<BoxCollider2D>());
            return true;
        }

        return false;
    }

    public SpriteRenderer GetSpriteRenderer()
    {
        return sr;
    }
}
