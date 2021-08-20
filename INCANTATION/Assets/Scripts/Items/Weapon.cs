using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Equipment
{
    public AttackType attackType;
    public float attackRange;
    public bool isCharged;

    public Weapon()
    {
        equipSlot = EquipmentSlot.weapon;
    }
}

public enum AttackType
{
    melee, range, freeRange
}
