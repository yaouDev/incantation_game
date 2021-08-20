using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : CharacterStats
{
    public Transform spawnPoint;
    private PlayerCombat playerCombat;

    //constants
    [SerializeField] private int baseDamage = 2;
    [SerializeField] private int baseArmor = 0;
    [SerializeField] private int baseAttackSpeed = 10;
    [SerializeField] private int baseMovementSpeed = 75;
    [SerializeField] private int baseMaxHealth = 200;

    void Start()
    {
        EquipmentManager.instance.onEquipmentChanged += OnEquipmentChanged;
        playerCombat = GetComponent<PlayerCombat>();
    }

    private void OnEquipmentChanged(Equipment newItem, Equipment oldItem)
    {
        if (newItem != null)
        {
            if (newItem is Weapon)
            {
                Weapon newWeapon = (Weapon)newItem;
                playerCombat.SetCurrentWeapon(newWeapon);
                playerCombat.attackRange = newWeapon.attackRange;

                if(newWeapon is FreeRangeWeapon)
                {
                    FreeRangeWeapon frw = (FreeRangeWeapon)newWeapon;
                    //VVV will store the sprite until a new freerange weapon is equipped
                    playerCombat.attackPoint.Find("FreeRangeTarget").GetComponent<SpriteRenderer>().sprite = frw.targetSprite;
                }
            }
            else if (newItem is Essence)
            {
                Essence newEssence = (Essence)newItem;
                playerCombat.SetEssenceType(newEssence.essenceType);
            }

            armor.AddModifier(newItem.armorModifer);
            damage.AddModifier(newItem.damageModifier);
            movementSpeed.AddModifier(newItem.moveSpeedModifier);
            attackSpeed.AddModifier(newItem.attackSpeedModifier);
        }

        if (oldItem != null)
        {
            armor.RemoveModifier(oldItem.armorModifer);
            damage.RemoveModifier(oldItem.damageModifier);
            movementSpeed.RemoveModifier(oldItem.moveSpeedModifier);
            attackSpeed.RemoveModifier(oldItem.attackSpeedModifier);
        }
    }

    public void SetClassStats(ClassSpecialization classSpec)
    {
        damage.SetBaseValue(baseDamage);
        armor.SetBaseValue(baseArmor);
        attackSpeed.SetBaseValue(baseAttackSpeed);
        movementSpeed.SetBaseValue(baseMovementSpeed);
        //...and health
        maxHealth.SetBaseValue(baseMaxHealth);

        //sets basestats - reconsider if needed or just for spells
        switch (classSpec)
        {
            case ClassSpecialization.mage:
                //mage things
                break;
            case ClassSpecialization.warrior:
                Debug.Log(maxHealth.GetBaseValue());
                //warrior things
                break;
            case ClassSpecialization.archer:
                movementSpeed.SetBaseValue(movementSpeed.GetBaseValue() + 50);
                //archer things
                break;
            case ClassSpecialization.summoner:
                //summoner things
                break;
            case ClassSpecialization.commoner:
                //commoner things
                break;
            default:
                break;
        }
    }

    public override void Die()
    {
        transform.position = spawnPoint.position;
        Heal(maxHealth.GetValue());
    }
}
