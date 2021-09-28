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
    public bool isAfflicted;

    private float bleedTimer;
    private float burnTimer;
    private float freezeTimer;
    private float afflictionTimer;

    [Header("Particles")]
    public ParticleSystem bloodParticles;
    public ParticleSystem fireParticles;
    public ParticleSystem afflictionParticles;

    public SpriteRenderer spriteRenderer;
    private CharacterStats stats;
    private Rigidbody2D rb;

    //if damage is instansiated by a different source with a different value, the most recent instance will change all remaining damage
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

            ParticleSystem.ShapeModule voidShape = afflictionParticles.shape;
            voidShape.radius = spriteRenderer.bounds.size.magnitude / 3f;
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
            case Effect.voidalAffliction:
                afflictionTimer = duration;
                SetAffliction();
                break;
            default:
                break;
        }
    }

    public void ApplyEffect(Effect effect, float duration)
    {
        ApplyEffect(effect, duration, 0);
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
            bloodParticles.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        }

        if (burnTimer > 0f)
        {
            burnTimer -= Time.deltaTime;
        }
        else
        {
            isBurning = false;
            CancelInvoke("Burn");
            fireParticles.Stop(true, ParticleSystemStopBehavior.StopEmitting);
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

        if (afflictionTimer > 0f)
        {
            afflictionTimer -= Time.deltaTime;
        }
        else
        {
            isAfflicted = false;
            CancelInvoke("Afflict");
            afflictionParticles.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        }
    }

    private void SetBleed()
    {
        bloodParticles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        //ParticleSystem.MainModule bloodMain = bloodParticles.main;
        //bloodMain.duration = bleedTimer;
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
        //ParticleSystem.MainModule fireMain = fireParticles.main;
        //fireMain.duration = burnTimer;
        fireParticles.Play();
        if (!isBurning)
        {
            InvokeRepeating("Burn", 0.25f, 0.5f);
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

    private void SetAffliction()
    {
        afflictionParticles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        //ParticleSystem.MainModule afflictionMain = afflictionParticles.main;
        //afflictionMain.duration = afflictionTimer;
        afflictionParticles.Play();
        if (!isAfflicted)
        {
            InvokeRepeating("Afflict", 0.125f, 0.25f);
            isAfflicted = true;
        }

        int modifier = stats.movementSpeed.GetValue() / 2;
        stats.StartCoroutine(stats.StatDrain(stats.movementSpeed, modifier, afflictionTimer));
    }

    //Attacks are all referenced from InvokeRepeating
    private void Bleed()
    {
        stats.TakeDamage(bleedDamage);
    }

    private void Burn()
    {
        if(burnDamage < 1)
        {
            burnDamage = 1;
        }

        stats.TakeDamage(burnDamage);
    }

    private void Freeze()
    {
        //the bool does things in other scripts
    }

    private void Afflict()
    {
        int afflictionDamage = stats.GetCurrentHealth() / 100;

        if (afflictionDamage < 1)
        {
            afflictionDamage = 1;
        }

        //hard coded to avoid super op things
        stats.TakeDamage(afflictionDamage);
    }
}

public enum Effect
{
    bleed, burn, freeze, slow, voidalAffliction
}
