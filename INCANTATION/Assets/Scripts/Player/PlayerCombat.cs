using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{

    public Animator animator;

    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask enemyLayers;
    public PlayerStats playerStats;
    private Rigidbody2D rb;

    private Camera cam;
    Vector2 mousePos;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerStats = gameObject.GetComponent<PlayerStats>();
        cam = Camera.main;
    }

    void Update()
    {
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetKeyDown(KeyCode.H))
        {
            Attack();
        }
    }


    //Change depending on melee/ranged
    private void FixedUpdate()
    {
        Vector2 lookDir = mousePos - rb.position;
        //float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
        //attackPoint.localEulerAngles = new Vector3(attackPoint.localPosition.x, attackPoint.localPosition.y, angle);
        float attackX = Mathf.Clamp(lookDir.x, -attackRange, attackRange);
        float attackY = Mathf.Clamp(lookDir.y, -attackRange, attackRange);
        attackPoint.localPosition = new Vector3(attackX, attackY);

        //rb.rotation = angle;
    }

    private void Attack()
    {
        //Play animation
        animator.SetTrigger("Attack");
        //Detect enemies
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
        //Damage them
        foreach (Collider2D hit in hitEnemies)
        {
            if(hit.gameObject.TryGetComponent<CharacterStats>(out CharacterStats enemy))
            {
                enemy.TakeDamage(playerStats.damage.GetValue());
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if(attackPoint == null)
        {
            return;
        }

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
