using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState : MonoBehaviour
{
    public int fullHP = 10;
    private int currentHP;

    //damage
    public int power = 5;

    //public Transform spawnPoint;

    // Start is called before the first frame update
    void Start()
    {
        currentHP = fullHP;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TakeDamage(int damage)
    {
        currentHP -= damage;
        if (currentHP <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}
