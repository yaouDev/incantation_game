using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <Notes>
/// Damage: Medium
/// Armor: None-Low
/// Move Speed: None-Low
/// Attack Speed: Fast
/// AttackRange: Depends on attack speed
/// 
/// Charge Rate: Slow/Medium
/// Charge Multiplier: 0.1 - 0.75 (given that the attackRange is baseAttackRange (1.1)
/// </Notes>

[CreateAssetMenu(fileName = "New Free Range Weapon", menuName = "Inventory/Weapon/Free Range Weapon")]
public class FreeRangeWeapon : Weapon
{
    [Header("Free Range")]
    public AnimatorOverrideController targetOverrides;

    public FreeRangeWeapon()
    {
        attackType = AttackType.freeRange;
        attackRange = 1.1f;
    }
}
