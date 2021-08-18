using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : CharacterStats
{
    public Transform spawnPoint;
    private PlayerCombat playerCombat;

    void Start()
    {
        EquipmentManager.instance.onEquipmentChanged += OnEquipmentChanged;
        playerCombat = GetComponent<PlayerCombat>();
    }

    private void OnEquipmentChanged(Equipment newItem, Equipment oldItem)
    {
        if(newItem != null)
        {
            if (newItem is Weapon)
            {
                Weapon newWeapon = (Weapon)newItem;
                playerCombat.SetAttackType(newWeapon.attackType);
            }

            armor.AddModifier(newItem.armorModifer);
            damage.AddModifier(newItem.damageModifier);
            movementSpeed.AddModifier(newItem.moveSpeedModifier);
            attackSpeed.AddModifier(newItem.attackSpeedModifier);
        }
        
        if(oldItem != null)
        {
            armor.RemoveModifier(oldItem.armorModifer);
            damage.RemoveModifier(oldItem.damageModifier);
            movementSpeed.RemoveModifier(oldItem.moveSpeedModifier);
            attackSpeed.RemoveModifier(oldItem.attackSpeedModifier);
        }
    }

    public override void Die()
    {
        transform.position = spawnPoint.position;
        Heal(maxHealth.GetValue());
    }
}
