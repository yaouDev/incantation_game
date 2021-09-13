using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack_Grab : WeaponAttack
{
    //I should cap how many hooks can be out at the same time, but it's too much fun

    public GameObject hook;
    public float hookSpeed;
    public Sprite hookGFX;
    public int hookDamage;

    private void Start()
    {
        InitializeWeapon();
    }

    private void Update()
    {
        if (canAttack() && Input.GetButton("Fire1"))
        {
            Grab();
            pcm.PlayerAttackAnimation();
        }
    }

    private void Grab()
    {
        Quaternion rot = Quaternion.Euler(new Vector3(0f, 0f, mm.GetAngle() - 90f));

        Hook hookInstance = Instantiate(hook, weapon.position, rot).GetComponent<Hook>();
        hookInstance.projectileVelocity = hookSpeed;

        hookInstance.GetSpriteRenderer().sprite = hookGFX;
        hookInstance.SetDamage(hookDamage);
        hookInstance.fetchDivider = 20f;
        hookInstance.grabDivider = 10f;

        AttackDelay();
    }
}
