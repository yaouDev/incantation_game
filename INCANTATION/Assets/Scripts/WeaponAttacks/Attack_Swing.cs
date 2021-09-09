using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack_Swing : WeaponAttack
{
    //This script is a mess. I was stressed when I wrote it, ok?
    //Reconsider if this script was a good idea...

    public GameObject hitbox;

    public float swingRadius = 1f;
    public float speed = 0.8f;

    private Vector3 startPos;
    private float coneEnd;
    private bool isAttacking;
    private bool right;

    void Start()
    {
        InitializeWeapon();

        hitbox.SetActive(false);
    }

    void Update()
    {
        if (Input.GetButton("Fire1") && canAttack() && !isAttacking)
        {
            //Swing
            isAttacking = true;
            hitbox.SetActive(true);
            startPos = hitbox.transform.localPosition;

            if (MouseManager.instance.IsLookingRight())
            {
                coneEnd = swingRadius;
                hitbox.transform.localPosition = new Vector3(hitbox.transform.localPosition.x - swingRadius, hitbox.transform.localPosition.y);
                right = true;
            }
            else
            {
                coneEnd = -swingRadius;
                hitbox.transform.localPosition = new Vector3(hitbox.transform.localPosition.x + swingRadius, hitbox.transform.localPosition.y);
                right = false;
            }
        }

        if (isAttacking)
        {
            Swing();
            hitbox.transform.rotation = attackPoint.rotation;

            if (hitbox.transform.localPosition.x > coneEnd && right)
            {
                TurnOffAttack();
            }
            else if (hitbox.transform.localPosition.x < coneEnd && !right)
            {
                TurnOffAttack();
            }
        }
    }

    private void TurnOffAttack()
    {
        isAttacking = false;
        hitbox.transform.localPosition = startPos;
        hitbox.SetActive(false);
        AttackDelay();
    }

    private void Swing()
    {
        if (right)
        {
            hitbox.transform.localPosition += new Vector3(speed, 0f);
        }
        else
        {
            hitbox.transform.localPosition -= new Vector3(speed, 0f);
        }
    }
}
