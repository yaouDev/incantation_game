using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{

    public int fullHP = 10;
    [SerializeField] private int currentHP;

    public Transform spawnPoint;

    public float invulnerableTime = 3f;
    [SerializeField] private float damageTimer;

    // Start is called before the first frame update
    void Start()
    {
        currentHP = fullHP;
    }

    // Update is called once per frame
    void Update()
    {
        if(damageTimer > 0)
        {
            damageTimer -= Time.deltaTime;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.TryGetComponent<EnemyState>(out EnemyState enemy))
        {
            if(damageTimer <= 0)
            {
                TakeDamage(enemy.power);
                damageTimer = invulnerableTime;
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<EnemyState>(out EnemyState enemy))
        {
            if (damageTimer <= 0)
            {
                TakeDamage(enemy.power);
                damageTimer = invulnerableTime;
            }
        }
    }

    public void TakeDamage(int damage)
    {
        currentHP -= damage;
        if(currentHP <= 0)
        {
            Debug.Log("Respawned!");
            Respawn();
        }
    }

    public void Respawn()
    {
        transform.position = spawnPoint.position;

        if (currentHP <= 0)
        {
            currentHP = fullHP;
        }
    }
}
