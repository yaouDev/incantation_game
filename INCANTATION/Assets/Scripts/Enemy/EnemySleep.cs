using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySleep : MonoBehaviour
{
    public float wakeUpTime = 1f;
    public float noticeRange = 5f;

    public bool isAwake;
    private bool wakingUp;

    private GameObject player;
    private EnemyStats stats;
    private Rigidbody2D rb;
    private EnemyMovement movement;

    private void Start()
    {
        player = PlayerManager.instance.player;
        stats = GetComponent<EnemyStats>();
        rb = GetComponent<Rigidbody2D>();
        movement = GetComponent<EnemyMovement>();
    }

    private void Update()
    {
        if (!isAwake)
        {
            FreezeMovement();

            float distance = Vector3.Distance(player.transform.position, gameObject.transform.position);
            if (distance <= noticeRange && !isAwake && !wakingUp || stats.GetCurrentHealth() != stats.maxHealth.GetValue() && !isAwake && !wakingUp)
            {
                wakingUp = true;
                StartCoroutine(WakeUp());
            }
        }
    }

    public void FreezeMovement()
    {
        rb.velocity = Vector2.zero;
        //movement.isMobile = false;
    }

    private IEnumerator WakeUp()
    {
        //wake up animation

        float normalizedTime = 0f;
        while (normalizedTime <= 1f)
        {
            normalizedTime += Time.deltaTime / wakeUpTime;
            yield return null;
        }

        Debug.Log(gameObject.name + " woke up!");
        isAwake = true;
        wakingUp = false;
        movement.isMobile = true;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(gameObject.transform.position, noticeRange);
    }
}
