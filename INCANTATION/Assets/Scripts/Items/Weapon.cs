using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Equipment
{
    [HideInInspector]
    public AttackType attackType;

    public float attackRange;

    public Sprite attackPointGFX;
    public bool attackPointSpin;

    [Header("Weapon Script")]
    public GameObject specialWeapon;

    [Header("Charge-related")]
    //---Charge related---
    public bool isCharged;

    [Range(0.1f, 10f)]
    public float chargeMultiplier = 1f;

    [Range(0.1f, 6f)]
    public float chargeRate = 0.6f;
    //---Charge related end---

    [Header("Knockback")]
    //100-ish is fine
    public float knockbackPower;
    public float knockbackPlayerPower;

    public Weapon()
    {
        equipSlot = EquipmentSlot.weapon;
    }
}

public enum AttackType
{
    melee, range, freeRange
}
