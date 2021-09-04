using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Inventory/Weapon")]
public class Weapon : Equipment
{
    public float attackRange;

    public Sprite attackPointGFX;
    public bool attackPointSpin;
    public bool lockedCombat;

    [Header("Weapon Script")]
    public GameObject weaponAttack;

    [Header("Knockback")]
    //100-ish is fine
    public float knockbackPower;
    public float knockbackPlayerPower;

    public Weapon()
    {
        equipSlot = EquipmentSlot.weapon;
    }
}
