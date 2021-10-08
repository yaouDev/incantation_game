using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

[CreateAssetMenu(fileName = "Unnamed Faction", menuName = "Faction")]
public class Faction : ScriptableObject
{
    public new LocalizedString name;
    public LocalizedString description;
    public Color color;
    public Sprite dialogPanel;
}
