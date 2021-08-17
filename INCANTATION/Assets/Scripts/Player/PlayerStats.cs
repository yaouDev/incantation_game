using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : CharacterStats
{
    public Transform spawnPoint;

    void Start()
    {
        EquipmentManager.instance.onEquipmentChanged += OnEquipmentChanged;
    }

    private void OnEquipmentChanged(Equipment newItem, Equipment oldItem)
    {
        if(newItem != null)
        {
            armor.AddModifier(newItem.armorModifer);
            damage.AddModifier(newItem.damageModifier);
        }
        
        if(oldItem != null)
        {
            armor.RemoveModifier(oldItem.armorModifer);
            damage.RemoveModifier(oldItem.damageModifier);
        }
    }

    public override void Die()
    {
        transform.position = spawnPoint.position;
        SetCurrentHealth(maxHealth.GetValue());
    }
}
