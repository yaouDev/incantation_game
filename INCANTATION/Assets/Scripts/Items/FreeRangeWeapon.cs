using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Free Range Weapon", menuName = "Inventory/Weapon/Free Range Weapon")]
public class FreeRangeWeapon : Weapon
{
    public Sprite targetSprite;

    public FreeRangeWeapon(bool charge)
    {
        attackType = AttackType.freeRange;
        attackRange = 1f;
    }
}
