using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerStats))]
public class PlayerClass : MonoBehaviour
{
    //constants
    [SerializeField] private int baseMaxHealth = 100;
    [SerializeField] private int baseMovementSpeed = 75;

    public ClassSpecialization classSpec;
    private PlayerStats playerStats;

    // Start is called before the first frame update
    void Start()
    {
        playerStats = GetComponent<PlayerStats>();

        //remove if choice is to be present
        SetClass(classSpec);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetClass(ClassSpecialization newClassSpec)
    {
        classSpec = newClassSpec;

        //sets basestats
        //attackspeed not implementetd yet
        switch (classSpec)
        {
            case ClassSpecialization.mage:
                playerStats.damage.SetBaseValue(5);
                playerStats.armor.SetBaseValue(0);
                playerStats.attackSpeed.SetBaseValue(0);
                playerStats.movementSpeed.SetBaseValue(baseMovementSpeed);
                //...and health
                playerStats.maxHealth.SetBaseValue(baseMaxHealth);
                break;
            case ClassSpecialization.warrior:
                playerStats.damage.SetBaseValue(5);
                playerStats.armor.SetBaseValue(0);
                playerStats.attackSpeed.SetBaseValue(0);
                playerStats.movementSpeed.SetBaseValue(baseMovementSpeed);
                //...and health
                playerStats.maxHealth.SetBaseValue(baseMaxHealth);
                break;
            case ClassSpecialization.archer:
                playerStats.damage.SetBaseValue(4);
                playerStats.armor.SetBaseValue(0);
                playerStats.attackSpeed.SetBaseValue(0);
                playerStats.movementSpeed.SetBaseValue(baseMovementSpeed);
                //...and health
                playerStats.maxHealth.SetBaseValue(baseMaxHealth);
                break;
            case ClassSpecialization.summoner:
                playerStats.damage.SetBaseValue(2);
                playerStats.armor.SetBaseValue(0);
                playerStats.attackSpeed.SetBaseValue(0);
                playerStats.movementSpeed.SetBaseValue(baseMovementSpeed);
                //...and health
                playerStats.maxHealth.SetBaseValue(baseMaxHealth);
                break;
            case ClassSpecialization.commoner:
                playerStats.damage.SetBaseValue(1);
                playerStats.armor.SetBaseValue(0);
                playerStats.attackSpeed.SetBaseValue(0);
                playerStats.movementSpeed.SetBaseValue(baseMovementSpeed);
                //...and health
                playerStats.maxHealth.SetBaseValue(baseMaxHealth);
                break;
            default:
                break;
        }
    }
}

public enum ClassSpecialization
{
    mage, warrior, archer, summoner, commoner
}
