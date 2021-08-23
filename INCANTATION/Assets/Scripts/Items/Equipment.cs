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
    public string specialIncantation = "";

    public AnimatorOverrideController[] animatorOverride;

    public override void Use()
    {
        base.Use();

        for (int i = 0; i < Inventory.instance.inventoryItems.Length; i++)
        {
            if (Inventory.instance.inventoryItems[i] == this)
            {
                //if you have two of the same item you wont swap item
                int index = Inventory.instance.GetIndex(Inventory.instance.inventoryItems, this);
                Inventory.instance.inventoryItems[index] = null;
                Inventory.instance.equipment[(int)equipSlot] = this;
                Inventory.instance.filledSlots--;
                Inventory.instance.isFull = false;
                EquipmentManager.instance.Equip(this);
                return;
            }
        }

        for (int i = 0; i < Inventory.instance.equipment.Length; i++)
        {
            if(Inventory.instance.equipment[i] == this && !Inventory.instance.isFull)
            {
                Inventory.instance.equipment[(int)equipSlot] = null;
                EquipmentManager.instance.Unequip((int)equipSlot);
                return;
            }
        }
    }
}

public enum EquipmentSlot
{
    head, chest, legs, weapon, essence
}
