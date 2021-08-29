using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyMeleeAttack : MonoBehaviour
{
    public float noticeRange = 3f;
    private AIPath aipath;

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

    private void Start()
    {
        player = PlayerManager.instance.player;
        stats = GetComponent<EnemyStats>();
        rb = GetComponent<Rigidbody2D>();
        aipath = GetComponent<AIPath>();
        aipath.canMove = false;
        aipath.canSearch = false;
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(player.transform.position, gameObject.transform.position);
        if (distance <= attackRange && !isAttacking)
        {
            StartCoroutine(PrepareAttack());
        }
        else if(distance <= noticeRange && !isAwake || stats.GetCurrentHealth() != stats.maxHealth.GetValue() && !isAwake)
        {
            StartCoroutine(WakeUp());
        }

        if (isAwake)
        {
            Vector2 direction = new Vector2(player.transform.position.x, player.transform.position.y) - rb.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            attackPoint.transform.localPosition = player.transform.position;
            attackPoint.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle - 90f));
            attackPoint.transform.localPosition = Vector3.ClampMagnitude(direction, attackOffset);
        }
    }

    private IEnumerator PrepareAttack()
    {
        isAttacking = true;

        float normalizedTime = 0f;
        while (normalizedTime <= 1f)
        {
            normalizedTime += Time.deltaTime / lag;
            rb.velocity = Vector2.zero;
            aipath.canMove = false;
            yield return null;
        }

        aipath.canMove = true;

        Attack();

        isAttacking = false;
    }

    private void Attack()
    {
        Collider2D hitPlayer = Physics2D.OverlapCircle(attackPoint.transform.position, attackRange, playerLayer);
        if(hitPlayer == null)
        {
            return;
        }

        if (hitPlayer.gameObject.TryGetComponent<PlayerStats>(out PlayerStats player))
        {
            player.TakeDamage(stats.damage.GetValue(), knockbackPower, transform);
            Debug.Log(gameObject.name + " hit player");
        }
    }

    private IEnumerator WakeUp()
    {
        float normalizedTime = 0f;
        while (normalizedTime <= 1f)
        {
            normalizedTime += Time.deltaTime / wakeUpTime;
            yield return null;
        }

        aipath.canMove = true;
        aipath.canSearch = true;
        isAwake = true;
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
        {
            return;
        }

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(attackPoint.transform.position, attackRange);
    }
}
