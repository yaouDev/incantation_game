using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization;

public class IncantationRegisterEntry : MonoBehaviour
{
    public Faction faction { private set; get; }

    public Text nameText;
    public Text descText;
    public Text triggerText;
    public GameObject lockedState;

    public void Set(string name, string description, string trigger, Faction faction, bool locked)
    {
        nameText.text = name;
        descText.text = description;
        triggerText.text = trigger;
        this.faction = faction;

        SetLockedState(locked);
        SetFaction(faction);
    }

    public void SetLockedState(bool check)
    {
        lockedState.SetActive(check);
    }

    public void SetFaction(Faction newFaction)
    {
        if (newFaction != null)
        {
            faction = newFaction;

            nameText.color = faction.factionColor;
            descText.color = faction.factionColor;
            triggerText.color = faction.factionColor;
        }
    }
}
