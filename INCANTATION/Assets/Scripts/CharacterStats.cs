using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public abstract class CharacterStats : MonoBehaviour
{
    public ParticleSystem onHitParticles;

    public EffectState effectState;

    public DamagePopUp damagePopUp;

    public Stat maxHealth;
    protected int currentHealth;
    protected Rigidbody2D rb;

    public delegate void OnDamageTaken();
    public OnDamageTaken onDamageTakenCallback;

    protected List<Stat> buffableStats = new List<Stat>();
    public Stat damage;
    public Stat armor;
    public Stat attackSpeed;
    public Stat movementSpeed;

    private void Start()
    {
        currentHealth = maxHealth.GetValue();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Awake()
    {
        buffableStats.Add(damage);
        buffableStats.Add(armor);
        buffableStats.Add(attackSpeed);
        buffableStats.Add(movementSpeed);
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

    public virtual void TakeDamage(int damage)
    {
        ReduceHealth(damage);

        Color color;
        if (gameObject.CompareTag("Player"))
        {
            color = Color.red;
        }
        else
        {
            color = Color.yellow;
        }

        damagePopUp.Pop(color, damage);

        CheckIfDead();

        //take damage animation
        //take damage visual representation
        onHitParticles.Play();
        //add whitehit here?

        if (onDamageTakenCallback != null)
        {
            onDamageTakenCallback.Invoke();
        }
    }

    protected void ReduceHealth(int damage)
    {
        damage -= armor.GetValue();
        damage = Mathf.Clamp(damage, 0, int.MaxValue);

        currentHealth -= damage;
        Debug.Log(transform.name + " takes " + damage + " damage.");
    }

    protected void CheckIfDead()
    {
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    //Overload with knockback float
    public void TakeDamage(int damage, float power, Transform other)
    {
        TakeDamage(damage);

        //knockback on damage VVV hard coded duration
        if(power > 0f)
        {
            StartCoroutine(Knockback(0.1f, power, other));
        }
    }

    public virtual void Die()
    {
        //Die in some way
        //overwritten
        //death animation?
        Debug.Log(transform.name + " died.");
    }

    public void Heal(int health)
    {
        currentHealth = health;
    }

    //VVV set velocity to zero on collision with walls!
    public IEnumerator Knockback(float duration, float power, Transform other)
    {
        float normalizedTime = 0f;
        while (normalizedTime <= 1f)
        {
            normalizedTime += Time.deltaTime / duration;
            Vector2 direction = (other.transform.position - transform.position).normalized;
            rb.AddForce(-direction * power);
            yield return null;
        }

        rb.velocity = Vector2.zero;
    }

    /*
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
    }*/

    public async Task StatBoost(Stat stat, int modifier, float duration)
    {
        stat.AddModifier(modifier);

        var end = Time.time + duration;
        while (Time.time < end)
        {
            await Task.Yield();
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

    public int GetCurrentHealth()
    {
        return currentHealth;
    }
}
