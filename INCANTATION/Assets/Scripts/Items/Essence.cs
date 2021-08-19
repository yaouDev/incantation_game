using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Essence", menuName = ("Inventory/Essence"))]
public class Essence : Equipment
{
    public EssenceType essenceType;
    public float cooldown = 10f;

    public Essence()
    {
        equipSlot = EquipmentSlot.essence;
    }
}

public enum EssenceType
{
    none, speed
}
