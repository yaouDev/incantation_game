using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss01 : MonoBehaviour
{
    public GameObject projectile;
    public Sprite projectileGFX;

    public float attackInterval = 10f;
    private float normalizedTimer;

    [Header("Attack")]
    public float knockbackPower;
    public float projectileSpeed = 5f;
    public int projectileDamage = 1;
    public float projectileSpace = 10f;

    [Header("Move")]
    public float moveSpeed = 5f;

    private bool isAwake;
    private bool isAttacking;
    private GameObject player;
    private EnemyStats stats;
    private Collider2D col;
    private EffectState es;

    private float spinTimer;
    private float spinCircle;
    [SerializeField] private float spinInterval;
    private bool spin;

    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        stats = GetComponent<EnemyStats>();
        player = PlayerManager.instance.player;
        col = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
        es = stats.effectState;
    }

    private void FixedUpdate()
    {
        if (!stats.isDead && !es.isFrozen)
        {
            //SpinCircle
            if (spinTimer > 0f)
            {
                spinTimer -= Time.deltaTime;
            }
            else if (spin)
            {
                FireProjectile(projectileDamage, spinCircle);
                if (spinCircle > 0f)
                {
                    if (spinCircle >= 360 * 2)
                    {
                        spinTimer = spinInterval / 3;
                        spinCircle -= projectileSpace / 3;
                    }
                    else if (spinCircle >= 360 && spinCircle < 360 * 2)
                    {
                        spinTimer = spinInterval / 2;
                        spinCircle -= projectileSpace / 2;
                    }
                    else
                    {
                        spinTimer = spinInterval;
                        spinCircle -= projectileSpace;
                    }
                }
                else
                {
                    isAttacking = false;
                    spin = false;
                    spinTimer = 0f;
                }
            }

            isAwake = GetComponent<EnemySleep>().isAwake;

            if (isAwake)
            {
                //Move
                Vector2 distance = player.transform.position - gameObject.transform.position;
                rb.MovePosition(rb.position + distance * (moveSpeed / 10f) * Time.fixedDeltaTime);

                if (normalizedTimer <= 1f)
                {
                    normalizedTimer += Time.deltaTime / attackInterval;
                }
                else
                {
                    normalizedTimer = 0f;

                    //send projectiles
                    Attack(Random.Range(1, 5));
                }
            }
        }
    }

    protected void FireProjectile(int damage, float angle)
    {
        Quaternion rot = Quaternion.Euler(new Vector3(0f, 0f, angle));

        EnemyProjectile projectileInstance = Instantiate(projectile, gameObject.transform.position, rot).GetComponent<EnemyProjectile>();
        projectileInstance.projectileVelocity = projectileSpeed;

        Physics2D.IgnoreCollision(col, projectileInstance.gameObject.GetComponent<CapsuleCollider2D>());

        projectileInstance.GetSpriteRenderer().sprite = projectileGFX;
        projectileInstance.knockbackPower = knockbackPower;
        projectileInstance.SetDamage(damage);
    }

    private void Attack(int attack)
    {
        if (!isAttacking)
        {
            isAttacking = true;

            switch (attack)
            {
                case 1:
                    CircleProjectiles();
                    break;
                case 2:
                    SpinCircle(360);
                    break;
                case 3:
                    SpinCircle(360 * 2);
                    break;
                case 4:
                    SpinCircle(360 * 3);
                    break;
                default:
                    break;
            }
        }
    }

    private void CircleProjectiles()
    {
        print("circle");
        float circle = 360;

        while (circle > 0)
        {
            FireProjectile(projectileDamage, circle);
            circle -= projectileSpace;
        }

        isAttacking = false;
    }

    private void SpinCircle(float circle)
    {
        print("spin");
        spinCircle = circle;
        spinTimer = spinInterval;
        spin = true;
    }
}
