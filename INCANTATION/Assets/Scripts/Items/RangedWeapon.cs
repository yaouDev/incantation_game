using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <Notes>
/// Damage: Medium
/// Armor: None-Low
/// Move Speed: Low-Medium
/// Attack Speed: Medium-High --- MAX 85!
/// AttackRange: N/A
/// 
/// Charge Rate: Medium/Fast
/// Charge Multiplier: Low
/// </Notes>

[CreateAssetMenu(fileName = "New Ranged Weapon", menuName = "Inventory/Weapon/Ranged Weapon")]
public class RangedWeapon : Weapon
{
    [Header("Ranged")]
    public Sprite projectile;

    public RangedWeapon()
    {
        attackType = AttackType.range;
        attackRange = 0f;
    }
}
