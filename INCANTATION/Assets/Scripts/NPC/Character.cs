using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Player", menuName = "Character/Player")]
public class Character : ScriptableObject
{
    new public string name;
    public CharacterClassification characterClassification;
    public Sprite dialogGFX;
}
