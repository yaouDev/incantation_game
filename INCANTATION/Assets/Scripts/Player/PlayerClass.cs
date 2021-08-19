using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerStats))]
public class PlayerClass : MonoBehaviour
{
    public ClassSpecialization classSpec;
    private PlayerStats playerStats;

    // Start is called before the first frame update
    void Start()
    {
        playerStats = GetComponent<PlayerStats>();

        //remove if choice is to be present
        SetClass(classSpec);

        //spaghetti way of setting player health to full health. might break if above is removed
        playerStats.Heal(playerStats.maxHealth.GetBaseValue());
    }

    public void SetClass(ClassSpecialization newClassSpec)
    {
        classSpec = newClassSpec;

        playerStats.SetClassStats(classSpec);
    }
}

public enum ClassSpecialization
{
    mage, warrior, archer, summoner, commoner
}
