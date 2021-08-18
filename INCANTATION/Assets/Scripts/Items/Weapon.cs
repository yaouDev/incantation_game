using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = ("Inventory/Weapon"))]
public class Weapon : Equipment
{
    public AttackType attackType;
}

public enum AttackType
{
    melee, range, freeRange
}
