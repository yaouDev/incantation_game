using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scorch : MonoBehaviour
{
    public int damage = 1;
    public float interval = 1f;
    public float radius = 1f;
    public float lifetime;

    private float lifeTimer;
    private float normalizedTimer;

    private void Trigger()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(gameObject.transform.position, radius, LayerMask.GetMask("Enemy"));
        foreach(Collider2D collider in hitEnemies)
        {
            if(collider.gameObject.TryGetComponent(out EnemyStats enemy))
            {
                enemy.TakeDamage(damage);
            }
        }
    }

    private void FixedUpdate()
    {
        if(normalizedTimer <= 1f)
        {
            normalizedTimer += Time.deltaTime / interval;
        }
        else
        {
            Trigger();
            normalizedTimer = 0f;
        }

        if(lifetime > 0f)
        {
            lifeTimer += Time.deltaTime;
            if(lifeTimer >= lifetime)
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;

        Gizmos.DrawWireSphere(gameObject.transform.position, radius);
    }
}
