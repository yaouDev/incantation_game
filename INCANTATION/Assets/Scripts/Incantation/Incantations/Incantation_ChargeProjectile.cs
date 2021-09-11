using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Incantation_ChargeProjectile : Incantation
{
    public GameObject projectile;
    public Sprite projectileGFX;
    public float timeToFullCharge = 1f;

    public float scaleMultiplier = 2f;
    public float speedMultiplier = 2f;
    public float damageMultiplier = 100f;

    private float normalizedTimer;
    private bool canCast;

    private GameObject activeWeapon;

    private void Update()
    {
        if (canCast)
        {
            if (Input.GetButton("Fire1"))
            {
                Charge();
            }
            else if (Input.GetButtonUp("Fire1"))
            {
                FireProjectile();
                canCast = false;

                if (activeWeapon != null)
                {
                    activeWeapon.GetComponent<WeaponAttack>().manualAttackFreeze = false;
                }
            }
        }
    }


    public override void Cast()
    {
        activeWeapon = EquipmentManager.instance.currentWeaponAttack;
        PlayerCombatManager.instance.attackPoint.GetComponent<SpriteRenderer>().sprite = projectileGFX;

        normalizedTimer = 0f;
        canCast = true;

        if(activeWeapon != null)
        {
            activeWeapon.GetComponent<WeaponAttack>().manualAttackFreeze = true;
        }
    }

    private void Charge()
    {
        if (normalizedTimer <= 1f && canCast)
        {
            normalizedTimer += Time.deltaTime / timeToFullCharge;
            print(normalizedTimer);
        }
    }

    private void FireProjectile()
    {
        Vector2 lookDir = MouseManager.instance.GetLookDir();
        float angle = MouseManager.instance.GetAngle();

        Quaternion rot = Quaternion.Euler(new Vector3(0f, 0f, angle - 90f));

        Projectile projectileInstance = Instantiate(projectile, PlayerManager.instance.player.transform.position, rot).GetComponent<Projectile>();

        projectileInstance.projectileVelocity = 5f + (1 + normalizedTimer * speedMultiplier);
        projectileInstance.transform.localScale += Vector3.one * (normalizedTimer * scaleMultiplier);
        projectileInstance.GetSpriteRenderer().sprite = projectileGFX;
        projectileInstance.knockbackPower = projectileInstance.transform.localScale.x * 100f;
        projectileInstance.SetDamage(Mathf.RoundToInt(1 + normalizedTimer * damageMultiplier));
    }
}
