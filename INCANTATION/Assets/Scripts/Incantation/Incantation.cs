using UnityEngine;

public abstract class Incantation : MonoBehaviour
{
    new public string name;
    public string description;
    public Faction faction;
    public string trigger;

    public abstract void Cast();
}

public enum Faction
{
    one, two, three, four
}