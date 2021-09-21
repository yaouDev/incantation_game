using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Unnamed Faction", menuName = "Faction")]
public class Faction : ScriptableObject
{
    public string factionName;
    public string factionDescription;
    public Color factionColor;
    public Sprite factionDialogPanel;
}
