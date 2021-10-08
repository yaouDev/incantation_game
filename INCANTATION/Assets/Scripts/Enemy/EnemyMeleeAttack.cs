using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using Pathfinding;

public class EnemyMeleeAttack : MonoBehaviour
{
    public float noticeRange = 3f;
    //private AIPath aipath;

    public GameObject attackPoint;
    public float attackRange = 1.1f;
    public float attackOffset = 1.5f;
    public float lag = 0.5f;
    public float wakeUpTime = 1f;
    public float knockbackPower;

    public LayerMask playerLayer;
    private GameObject player;
    private bool isAwake;

    private EnemyStats stats;
    private bool isAttacking;

    private Rigidbody2D rb;
    private EnemyMovement movement;

    public Vector2 centerOffset;

    private void Start()
    {
        player = PlayerManager.instance.player;
        stats = GetComponent<EnemyStats>();
        rb = GetComponent<Rigidbody2D>();
        movement = GetComponent<EnemyMovement>();

        //aipath = GetComponent<AIPath>();
        //aipath.canMove = false;
        //aipath.canSearch = false;
    }

    // Update is called once per frame
    void Update()
    {
        isAwake = GetComponent<EnemySleep>().isAwake;

        if (!stats.isDead)
        {
            float distance = Vector3.Distance(player.transform.position, gameObject.transform.position + (Vector3)centerOffset);
            if (distance <= attackRange && !isAttacking)
            {
                StartCoroutine(PrepareAttack());
            }

            if (isAwake)
            {
                //Vector2 direction = new Vector2(player.transform.position.x, player.transform.position.y) - rb.position;
                Vector2 direction = (Vector2)player.transform.position - (rb.position + centerOffset);
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

                //attackPoint.transform.localPosition = player.transform.position;
                attackPoint.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle - 90f));
                attackPoint.transform.localPosition = (Vector3)centerOffset + Vector3.ClampMagnitude(direction, attackOffset);
            }
        }
        else
        {
            Stop();
        }
        
    }

    //why is the attackpoint center on object center

    private void Stop()
    {
        rb.velocity = Vector3.zero;
        movement.isMobile = false;
        //aipath.canMove = false;
        //aipath.canSearch = false;
    }

    private IEnumerator PrepareAttack()
    {
        isAttacking = true;

        //attack animation
        movement.isMobile = false;

        float normalizedTime = 0f;
        while (normalizedTime <= 1f)
        {
            normalizedTime += Time.deltaTime / lag * stats.attackSpeed.GetValue();
            rb.velocity = Vector2.zero;
            //aipath.canMove = false;
            yield return null;
        }

        //aipath.canMove = true;
        movement.isMobile = true;

        Attack();

        isAttacking = false;
    }

    private void Attack()
    {
        Collider2D hitPlayer = Physics2D.OverlapCircle(attackPoint.transform.position, attackRange, playerLayer);
        if(hitPlayer == null)
        {
            Debug.Log(gameObject.name + " missed!");
            return;
        }

        if (hitPlayer.gameObject.TryGetComponent(out PlayerStats player))
        {
            player.TakeDamage(stats.damage.GetValue(), knockbackPower, transform);
            GetComponent<EnemyMovement>().animator.SetTrigger("Attack");
            Debug.Log(gameObject.name + " hit player");
        }
    }

    //change bat collision to shadow, hitbox remains on bat

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        Vector2 position = rb != null ? rb.position : (Vector2)transform.position;

        Gizmos.DrawWireSphere(position + centerOffset, 0.25f);

        Gizmos.color = Color.white;

        Gizmos.DrawWireSphere(attackPoint.transform.position, attackRange);
    }
}
