using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public Stat maxHealth;
    private int currentHealth;

    public Stat damage;
    public Stat armor;

    private void Awake()
    {
        currentHealth = maxHealth.GetValue();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            TakeDamage(10);
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

    public void SetCurrentHealth(int health)
    {
        currentHealth = health;
    }
}
