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