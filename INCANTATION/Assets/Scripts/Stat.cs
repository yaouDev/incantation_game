using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Stat
{
    [SerializeField] private int baseValue;
    [SerializeField] private int minValue;
    [SerializeField] private int maxValue = 9999;


    private List<int> modifiers = new List<int>();

    public int GetValue()
    {
        int finalValue = baseValue;
        modifiers.ForEach(x => finalValue += x);

        if(finalValue > maxValue)
        {
            finalValue = maxValue;
        }
        else if(finalValue < minValue)
        {
            finalValue = minValue;
        }

        return finalValue;
    }

    public void SetBaseValue(int value)
    {
        if(value >= minValue)
        {
            baseValue = value;
        }
    }

    public int GetBaseValue()
    {
        return baseValue;
    }

    public int GetMaxValue()
    {
        return maxValue;
    }

    public int GetMinValue()
    {
        return minValue;
    }

    public void AddModifier(int modifier)
    {
        if(modifier != 0)
        {
            modifiers.Add(modifier);
        }
    }

    public void RemoveModifier(int modifier)
    {
        if(modifier != 0)
        {
            modifiers.Remove(modifier);
        }
    }
        
}
