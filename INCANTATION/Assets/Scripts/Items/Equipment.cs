using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Equipment", menuName = ("Inventory/Equipment"))]
public class Equipment : Item
{
    public EquipmentSlot equipSlot;

    [Header("Stat modifiers")]
    public int armorModifer;
    public int damageModifier;
    public int moveSpeedModifier;
    public int attackSpeedModifier;

    [Header("Incantation")]
    public string specialIncantation = "";

    [Header("Animation")]
    public AnimatorOverrideController[] animatorOverride;

    [Header("Light Information")]
    public bool turnOnLight;
    public float lightIntensity = 1f;
    public float lightInnerRange = 0f;
    public float lightOuterRange = 1f;
    public Color lightColor = Color.white;
    public Vector3 lightOffset;

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
