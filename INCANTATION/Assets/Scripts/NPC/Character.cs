using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

[CreateAssetMenu(fileName = "Player", menuName = "Character/Player")]
public class Character : ScriptableObject
{
    new public LocalizedString name;
    public CharacterClassification characterClassification;
    public Sprite dialogGFX;
}
