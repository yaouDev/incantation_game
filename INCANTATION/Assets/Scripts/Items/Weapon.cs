using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = ("Inventory/Weapon"))]
public class Weapon : Equipment
{
    public AttackType attackType;
    public float attackRange;
    public Sprite projectile;

    public Weapon()
    {
        equipSlot = EquipmentSlot.weapon;
    }
}

public enum AttackType
{
    melee, range, freeRange
}
