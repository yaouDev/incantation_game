using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Ranged Weapon", menuName = "Inventory/Weapon/Ranged Weapon")]
public class RangedWeapon : Weapon
{
    public Sprite projectile;

    public RangedWeapon()
    {
        attackType = AttackType.range;
        attackRange = 0f;
    }
}
