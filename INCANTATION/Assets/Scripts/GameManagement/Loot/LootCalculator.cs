using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootCalculator : MonoBehaviour
{
    public static LootCalculator instance;

    private void Awake()
    {
        #region Singleton
        if(instance != null)
        {
            Debug.LogWarning("More than one loot calculators found!");
            return;
        }
        instance = this;
        #endregion
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Rarity Calculcate()
    {
        float value = Random.Range(0, 100);

        if(value <= 50f)
        {
            return Rarity.common;
        }
        else if (value >= 50f && value <= 74f)
        {
            return Rarity.uncommon;
        }
        else if (value >= 75f && value <= 87.5f)
        {
            return Rarity.rare;
        }
        else if(value >= 87.6f && value <= 93.85f)
        {
            return Rarity.mythic;
        }
        else if(value >= 93.86 && value <= 97f)
        {
            return Rarity.legendary;
        }
        else
        {
            //collect the inbetween-numbers + last 3%
            return Rarity.special;
        }
    }

    public void TestDrop()
    {
        Rarity rarity = Calculcate();
        Debug.Log("You got a " + rarity + "!");
    }
}
