using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

public abstract class Incantation : ScriptableObject
{
    new public LocalizedString name;
    public LocalizedString description;
    public Faction faction;
    public string trigger;

    public abstract void Cast();
}

public class IncantationComparator : Comparer<Incantation>
{
    public override int Compare(Incantation x, Incantation y)
    {
        //if one is null and the other isn't, return the non-null one
        if(x.faction != null && y.faction == null)
        {
            if(x.faction.name == null || x.faction.description == null)
            {
                Debug.LogWarning("Faction info is null for " + x.name.GetLocalizedString());
            }
            return 1;
        }
        else if(x.faction == null && y.faction != null)
        {
            if (y.faction.name == null || y.faction.description == null)
            {
                Debug.LogWarning("Faction info is null for " + y.name.GetLocalizedString());
            }
            return -1;
        }

        //if the faction is equal (same faction or both null) compare incantation names
        if (x.faction == null && y.faction == null || x.faction.Equals(y.faction))
        {
            return x.name.GetLocalizedString().CompareTo(y.name.GetLocalizedString());
        }

        //both have a faction, therefor return their comparison
        return x.faction.name.GetLocalizedString().CompareTo(y.faction.name.GetLocalizedString());
    }
}