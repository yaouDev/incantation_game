using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Melee Weapon", menuName = "Inventory/Weapon/Melee Weapon")]
public class MeleeWeapon : Weapon
{
    public MeleeWeapon()
    {
        attackType = AttackType.melee;
        attackRange = 1.1f;
    }
}
