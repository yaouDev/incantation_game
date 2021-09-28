using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

[System.Serializable]
public class Dialog
{
    public Character speaker;

    public LocalizedString[] sentences;

    public bool isEnd;

    [Header("Dialog Choice")]
    public bool hasChoice;
    public LocalizedString[] choices;
    public int[] jumps = new int[3];
}
