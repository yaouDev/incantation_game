using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack_Bow : ProjectileCombat
{
    //The further back you charge, the better indication you get, and more speed. 

    private bool isAttacking;

    private Vector3 startPos;
    private Vector3 endPos;
    public float minPower = 1f;
    public float maxPower = 100f;

    private float autoFireTimer;
    public float autoFire = 4f;
    public float damageMultiplier = 10f;

    private float normalizedTimer;

    public LineRenderer gfx;
    public LineRenderer indicator;

    protected Vector2 mousePos;

    void Start()
    {
        InitializeWeapon();

        cam = Camera.main;
        gfx.gameObject.SetActive(false);
    }

    void Update()
    {
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);

        if (canAttack() && Input.GetButton("Fire1") && !isAttacking)
        {
            autoFireTimer = autoFire;

            isAttacking = true;
            gfx.gameObject.SetActive(true);
            indicator.startColor = new Color(1f, 0.5f, 0f);
        }
        else if (Input.GetButtonUp("Fire1") && isAttacking || autoFireTimer <= 0f && isAttacking)
        {
            float power = Vector3.Distance(startPos, endPos);
            power = Mathf.Clamp(power, minPower, maxPower);

            //use knockback??
            //knockbackPower = power;
            projectileSpeed = -power;
            FireProjectile(Mathf.RoundToInt(playerStats.damage.GetValue() * (damageMultiplier * normalizedTimer)));
            isAttacking = false;
            gfx.gameObject.SetActive(false);
            normalizedTimer = 0f;
            autoFireTimer = 0f;
        }

        if (isAttacking)
        {
            startPos = transform.position;
            endPos = mousePos;

            gfx.SetPosition(0, endPos);
            gfx.SetPosition(1, startPos);

            Vector3 indiEnd = endPos - startPos;
            indiEnd = indiEnd * -1f;
            indicator.SetPosition(0, startPos + indiEnd);
            indicator.SetPosition(1, startPos);

            indicator.startColor += new Color(0f, 0f, normalizedTimer * Time.deltaTime, 0f);
        }
    }

    private void FixedUpdate()
    {
        attackSpeedTimer();

        if (autoFireTimer > 0f)
        {
            autoFireTimer -= Time.deltaTime;
            normalizedTimer += Time.deltaTime / autoFire;
        }
    }
}
