using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public Stat maxHealth;
    private int currentHealth;

    private List<Stat> buffableStats = new List<Stat>();
    public Stat damage;
    public Stat armor;
    public Stat attackSpeed;
    public Stat movementSpeed;

    private void Start()
    {
        print(maxHealth.GetValue());
        currentHealth = maxHealth.GetValue();
    }

    private void Awake()
    {
        buffableStats.Add(damage);
        buffableStats.Add(armor);
        buffableStats.Add(attackSpeed);
        buffableStats.Add(movementSpeed);
    }

    private void Update()
    {
        //testing
        if (Input.GetKeyDown(KeyCode.T))
        {
            TakeDamage(10);
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            StartCoroutine(StatBoost(armor, 5, 5f));
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            StartCoroutine(StatDrain(armor, 5, 5f));
        }
    }

    private void LateUpdate()
    {
        if (gameObject.CompareTag("Player"))
        {
            gameObject.GetComponent<PlayerMovement>().moveSpeed = movementSpeed.GetValue();
        }
        else if (gameObject.CompareTag("Enemy"))
        {
            //add enemymovement
        }
    }

    public void TakeDamage(int damage)
    {
        damage -= armor.GetValue();
        damage = Mathf.Clamp(damage, 0, int.MaxValue);

        currentHealth -= damage;
        Debug.Log(transform.name + " takes " + damage + " damage.");

        if(currentHealth <= 0)
        {
            Die();
        }
    }

    public virtual void Die()
    {
        //Die in some way
        //overwritten
        Debug.Log(transform.name + " died.");
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }

    public void Heal(int health)
    {
        currentHealth = health;
    }

    public IEnumerator StatBoost(Stat stat, int modifier, float duration)
    {
        stat.AddModifier(modifier);

        float normalizedTime = 0f;
        while(normalizedTime <= 1f)
        {
            normalizedTime += Time.deltaTime / duration;
            yield return null;
        }

        stat.RemoveModifier(modifier);
    }

    public IEnumerator StatDrain(Stat stat, int modifier, float duration)
    {
        stat.AddModifier(-modifier);

        float normalizedTime = 0f;
        while (normalizedTime <= 1f)
        {
            normalizedTime += Time.deltaTime / duration;
            yield return null;
        }

        stat.RemoveModifier(-modifier);
    }
}
