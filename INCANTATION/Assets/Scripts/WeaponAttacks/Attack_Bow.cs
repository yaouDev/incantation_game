using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack_Bow : ProjectileCombat
{
    private bool isAttacking;

    private Vector3 startPos;
    private Vector3 endPos;
    public float minPower = 1f;
    public float maxPower = 100f;

    private float autoFireTimer;
    public float autoFire = 4f;

    public LineRenderer gfx;

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
        }
        else if (Input.GetButtonUp("Fire1") && isAttacking || autoFireTimer <= 0f && isAttacking)
        {
            float power = Vector3.Distance(startPos, endPos);
            power = Mathf.Clamp(power, minPower, maxPower);

            projectileSpeed = -power;
            FireProjectile(Mathf.RoundToInt(power));
            isAttacking = false;
            gfx.gameObject.SetActive(false);
        }

        if (isAttacking)
        {
            startPos = transform.position;
            endPos = mousePos;

            gfx.SetPosition(0, endPos);
            gfx.SetPosition(1, startPos);
        }
    }

    private void FixedUpdate()
    {
        attackSpeedTimer();

        if (autoFireTimer > 0f)
        {
            autoFireTimer -= Time.deltaTime;
        }
    }
}
