using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Equipment", menuName = ("Inventory/Equipment"))]
public class Equipment : Item
{
    public EquipmentSlot equipSlot;

    public int armorModifer;
    public int damageModifier;
    public int moveSpeedModifier;
    public int attackSpeedModifier;

    public AnimatorOverrideController[] animatorOverride;

    public override void Use()
    {
        base.Use();

        EquipmentManager.instance.Equip(this);
        RemoveFromInventory();
    }
}

public enum EquipmentSlot
{
    head, chest, legs, weapon, essence
}
