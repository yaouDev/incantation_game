using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ProjectileCombat : UnlockedCombat
{
    /*
    [SerializeField] private GameObject projectile;
    [SerializeField] private SpriteRenderer projectileGFX;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected void FireProjectile()
    {
        //VVV change to scale with something
        float projectileSpeed = 10f;
        RangedWeapon rangedWeapon = (RangedWeapon)currentWeapon;
        projectileSpeed = rangedWeapon.projectileSpeed;

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

        projectileInstance.GetSpriteRenderer().sprite = rangedWeapon.projectile;
        projectileInstance.knockbackPower = rangedWeapon.knockbackPower;
        projectileInstance.SetDamage(projectileDamage);

        //Change to scale with something?VVV
        //projectileInstance.transform.localScale = new Vector3(projectileInstance.transform.localScale.x + currentCharge, projectileInstance.transform.localScale.y + currentCharge, projectileInstance.transform.localScale.z);

        AttackDelay();
        
    }*/
}
