using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileCombat : WeaponAttack
{
    [SerializeField] private GameObject projectile;
    [SerializeField] private Sprite projectileGFX;
    public float projectileSpeed;

    private Vector2 mousePos;
    private Camera cam;

    //general
    private float knockbackPower;

    void Start()
    {
        InitializeWeapon();

        cam = Camera.main;

        //general
        Weapon currentWeapon = (Weapon)EquipmentManager.instance.currentEquipment[(int)EquipmentSlot.weapon];
        knockbackPower = currentWeapon.knockbackPower;
    }

    void Update()
    {
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);

        if(canAttack() && Input.GetButton("Fire1") && damageTimer <= 0f)
        {
            FireProjectile();
            pcm.PlayerAttackAnimation();
        }
    }

    protected void FireProjectile()
    {
        //VVV change to scale with something
        projectileSpeed = 10f;

        //rb.position -> weapon.position?
        Vector2 lookDir = mousePos - new Vector2(weapon.position.x, weapon.position.y);
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;

        Quaternion rot = Quaternion.Euler(new Vector3(0f, 0f, angle - 90f));

        Projectile projectileInstance = Instantiate(projectile, weapon.position, rot).GetComponent<Projectile>();
        projectileInstance.projectileVelocity = projectileSpeed;

        //VVV change to scale with something
        int projectileDamage = playerStats.damage.GetValue();

        //fail-safe
        if (projectileDamage < playerStats.damage.GetValue())
        {
            projectileDamage = playerStats.damage.GetValue();
        }

        projectileInstance.GetSpriteRenderer().sprite = projectileGFX;
        projectileInstance.knockbackPower = knockbackPower;
        projectileInstance.SetDamage(projectileDamage);

        //Change to scale with something?VVV
        //projectileInstance.transform.localScale = new Vector3(projectileInstance.transform.localScale.x + currentCharge, projectileInstance.transform.localScale.y + currentCharge, projectileInstance.transform.localScale.z);

        AttackDelay();
    }
}
