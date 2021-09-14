using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack_Spin : WeaponAttack
{
    public float radius = 1.5f;
    public GameObject hitbox;

    private float rotateSpeed = 5f;

    private Vector2 center;
    private float angle;

    private int direction;
    private bool isAttacking;

    private void Start()
    {
        InitializeWeapon();

        hitbox.SetActive(false);
        rotateSpeed = playerStats.attackSpeed.GetValue() / 2f;
    }

    //A litte bit messy
    void Update()
    {
        if (canAttack())
        {
            if (Input.GetButtonDown("Fire1"))
            {
                ActivateHitbox();
                direction = -1;
            }
            else if (Input.GetButtonDown("Fire2"))
            {
                ActivateHitbox();
                direction = 1;
            }
        }

        if (Input.GetButtonUp("Fire1") && !Input.GetButton("Fire2") || Input.GetButtonUp("Fire2") && !Input.GetButton("Fire1"))
        {
            hitbox.gameObject.SetActive(false);
            isAttacking = false;
            direction = 0;
        }

        if (isAttacking)
        {
            Spin(direction);
        }
    }

    private void ActivateHitbox()
    {
        hitbox.gameObject.SetActive(true);
        AttackDelay();
        isAttacking = true;
    }

    private void Spin(int clockwise)
    {
        center = pcm.player.transform.position;
        angle += (rotateSpeed * clockwise) * Time.deltaTime;

        var offset = new Vector2(Mathf.Sin(angle), Mathf.Cos(angle)) * radius;
        hitbox.transform.position = center + offset;
    }
}
