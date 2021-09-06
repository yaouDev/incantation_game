using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public Stat maxHealth;
    protected int currentHealth;
    private Rigidbody2D rb;

    public GameObject damagePopUp;

    public delegate void OnDamageTaken();
    public OnDamageTaken onDamageTakenCallback;

    public bool isBoosted { get; private set; }
    public bool isDrained { get; private set; }

    private List<Stat> buffableStats = new List<Stat>();
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

    private void Update()
    {


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

        //damage popup
        damagePopUp.GetComponent<DamagePopUp>().text.text = "-" + damage;
        DamagePopUp popUpInstance = Instantiate(damagePopUp, transform.position, transform.rotation).GetComponent<DamagePopUp>();
        popUpInstance.text.gameObject.GetComponent<UIFollowGameObject>().target = gameObject;

        if (currentHealth <= 0)
        {
            Die();
        }

        //take damage animation

        if (onDamageTakenCallback != null)
        {
            onDamageTakenCallback.Invoke();
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
           // rb.isKinematic = false;
            normalizedTime += Time.deltaTime / duration;
            Vector2 direction = (other.transform.position - transform.position).normalized;
            rb.AddForce(-direction * power);
            yield return null;
        }

        /*
        while (duration > 0f)
        {
            duration -= Time.deltaTime;
            Vector2 direction = (other.transform.position - transform.position).normalized;
            //rb.AddForce(-direction * power, ForceMode2D.Impulse);
            rb.MovePosition(-direction * power);
        }*/

        //rb.isKinematic = true;
        rb.velocity = Vector2.zero;

        //yield return null;
    }

    public IEnumerator StatBoost(Stat stat, int modifier, float duration)
    {
        stat.AddModifier(modifier);
        isBoosted = true;

        float normalizedTime = 0f;
        while(normalizedTime <= 1f)
        {
            normalizedTime += Time.deltaTime / duration;
            yield return null;
        }

        stat.RemoveModifier(modifier);
        isBoosted = false;
    }

    public IEnumerator StatDrain(Stat stat, int modifier, float duration)
    {
        stat.AddModifier(-modifier);
        isDrained = true;

        float normalizedTime = 0f;
        while (normalizedTime <= 1f)
        {
            normalizedTime += Time.deltaTime / duration;
            yield return null;
        }

        stat.RemoveModifier(-modifier);
        isDrained = false;
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }
}
