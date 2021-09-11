using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileCombat : WeaponAttack
{
    [SerializeField] protected GameObject projectile;
    [SerializeField] protected Sprite projectileGFX;
    public float projectileSpeed = 10f;

    protected Camera cam;

    protected Vector2 lookDir;
    protected float angle;
    

    void Start()
    {
        InitializeWeapon();

        cam = Camera.main;
    }

    void Update()
    {
        if(canAttack() && Input.GetButton("Fire1"))
        {
            FireProjectile(playerStats.damage.GetValue());
            pcm.PlayerAttackAnimation();
        }
    }

    protected void FireProjectile(int damage)
    {
        //VVV change to scale with something

        //rb.position -> weapon.position?
        //lookDir = mousePos - new Vector2(weapon.position.x, weapon.position.y);
        //angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;

        lookDir = MouseManager.instance.GetLookDir();
        angle = MouseManager.instance.GetAngle();

        Quaternion rot = Quaternion.Euler(new Vector3(0f, 0f, angle - 90f));

        Projectile projectileInstance = Instantiate(projectile, weapon.position, rot).GetComponent<Projectile>();
        projectileInstance.projectileVelocity = projectileSpeed;

        //fail-safe
        if (damage < playerStats.damage.GetValue())
        {
            damage = playerStats.damage.GetValue();
        }

        projectileInstance.GetSpriteRenderer().sprite = projectileGFX;
        projectileInstance.knockbackPower = knockbackPower;
        projectileInstance.SetDamage(damage);

        //Change to scale with something?VVV
        //projectileInstance.transform.localScale = new Vector3(projectileInstance.transform.localScale.x + currentCharge, projectileInstance.transform.localScale.y + currentCharge, projectileInstance.transform.localScale.z);

        AttackDelay();
    }
}
