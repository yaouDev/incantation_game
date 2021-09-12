using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    [HideInInspector] public float projectileVelocity;
    [HideInInspector] public float knockbackPower;
    private bool collided;
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private LayerMask lm;

    public int damage = 0;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        lm = gameObject.layer;

        Physics2D.IgnoreLayerCollision(lm, lm);

        Destroy(gameObject, 15f);
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
        if (collision.gameObject.TryGetComponent(out PlayerStats player) && !collided)
        {
            if (knockbackPower > 0f)
            {
                player.TakeDamage(damage, knockbackPower, transform);
            }
            else
            {
                player.TakeDamage(damage);
            }

            collided = true;
        }
        else if (IgnoreCollision(collision))
        {
            return;
        }

        //projectile death animation
        gameObject.SetActive(false);
        rb.velocity = Vector2.zero;
        Destroy(gameObject, 1f);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (IgnoreCollision(collision))
        {
            return;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (IgnoreCollision(collision))
        {
            return;
        }
    }

    private bool IgnoreCollision(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Physics2D.IgnoreCollision(collision.collider, GetComponent<CapsuleCollider2D>());
            return true;
        }

        return false;
    }

    public SpriteRenderer GetSpriteRenderer()
    {
        return sr;
    }
}
