using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <Notes>
/// Damage: High
/// Armor: Low
/// Move Speed: Depends on damage
/// Attack Speed: Medium
/// AttackRange: Medium/Depends
/// 
/// Charge Rate: Slow
/// Charge Multiplier: High
/// </Notes>

[CreateAssetMenu(fileName = "New Melee Weapon", menuName = "Inventory/Weapon/Melee Weapon")]
public class MeleeWeapon : Weapon
{
    //[Header("Melee")]

    public MeleeWeapon()
    {
        attackType = AttackType.melee;
        attackRange = 1.1f;
    }
}
