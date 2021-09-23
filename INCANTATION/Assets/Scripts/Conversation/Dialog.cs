using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialog
{
    public Character speaker;

    [TextArea(3, 10)]
    public string[] sentences;

    public bool isEnd;

    [Header("Dialog Choice")]
    public bool hasChoice;
    public string[] choices;
    public int[] jumps = new int[3];
}
