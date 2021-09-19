using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectState : MonoBehaviour
{
    //yes, i couldve made an effect class and done everything via them but it seemed unnecessary to make a new class for each effect, and I'm inexperienced.

    [Header("States")]
    public bool isBleeding;
    public bool isBurning;
    public bool isFrozen;

    private float bleedTimer;
    private float burnTimer;
    private float freezeTimer;

    [Header("Particles")]
    public ParticleSystem bloodParticles;
    public ParticleSystem fireParticles;

    public SpriteRenderer spriteRenderer;
    private CharacterStats stats;
    private Rigidbody2D rb;

    private int bleedDamage;
    private int burnDamage;

    [Header("Effect-related")]
    public Color freezeColor = new Color(0.5f, 0.5f, 1f);

    private void Start()
    {
        stats = transform.parent.GetComponent<CharacterStats>();
        rb = transform.parent.GetComponent<Rigidbody2D>();

        //set the appropriate bounds for each effect
        if(spriteRenderer != null)
        {
            ParticleSystem.ShapeModule bloodShape = bloodParticles.shape;
            bloodShape.radius = spriteRenderer.bounds.size.magnitude / 3f;
            bloodShape.position = new Vector3(0, bloodShape.radius / 2, 0);

            ParticleSystem.ShapeModule fireShape = fireParticles.shape;
            fireShape.radius = spriteRenderer.bounds.size.magnitude / 3f;
        }
    }

    public void ApplyEffect(Effect effect, float duration, int modifier)
    {
        switch (effect)
        {
            case Effect.bleed:
                bleedTimer = duration;
                bleedDamage = modifier;
                SetBleed();
                break;
            case Effect.burn:
                burnTimer = duration;
                burnDamage = modifier;
                SetBurn();
                break;
            case Effect.freeze:
                freezeTimer = duration;
                SetFreeze();
                break;
            case Effect.slow:
                SetSlow(duration, modifier);
                break;
            default:
                break;
        }
    }

    private void FixedUpdate()
    {
        if(bleedTimer > 0f)
        {
            bleedTimer -= Time.deltaTime;
        }
        else
        {
            isBleeding = false;
            CancelInvoke("Bleed");
        }

        if (burnTimer > 0f)
        {
            burnTimer -= Time.deltaTime;
        }
        else
        {
            isBurning = false;
            CancelInvoke("Burn");
        }

        if (freezeTimer > 0f)
        {
            freezeTimer -= Time.deltaTime;
        }
        else
        {
            isFrozen = false;

            if(spriteRenderer != null)
            {
                spriteRenderer.color = Color.white;
            }
        }
    }

    private void SetBleed()
    {
        bloodParticles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        ParticleSystem.MainModule bloodMain = bloodParticles.main;
        bloodMain.duration = bleedTimer;
        bloodParticles.Play();
        if (!isBleeding)
        {
            InvokeRepeating("Bleed", 0.5f, 1f);
            isBleeding = true;
        }

        int modifier = stats.movementSpeed.GetValue() / 4;
        stats.StartCoroutine(stats.StatDrain(stats.movementSpeed, modifier, bleedTimer));
    }

    private void SetBurn()
    {
        fireParticles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        ParticleSystem.MainModule fireMain = fireParticles.main;
        fireMain.duration = burnTimer;
        fireParticles.Play();
        if (!isBurning)
        {
            InvokeRepeating("Burn", 0.5f, 0.5f);
            isBurning = true;
        }
    }

    private void SetFreeze()
    {
        isFrozen = true;

        if (spriteRenderer != null)
        {
            spriteRenderer.color = freezeColor;
        }
    }

    private void SetSlow(float duration, int m)
    {
        int modifier = stats.movementSpeed.GetValue() / m;
        stats.StartCoroutine(stats.StatDrain(stats.movementSpeed, modifier, duration));
    }

    private void Bleed()
    {
        stats.TakeDamage(bleedDamage);
    }

    private void Burn()
    {
        stats.TakeDamage(burnDamage);
    }

    private void Freeze()
    {
        //the bool does things in other scripts
    }
}

public enum Effect
{
    bleed, burn, freeze, slow, voidalAffliction
}
