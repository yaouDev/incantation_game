using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialog
{
    public Character speaker;

    [TextArea(3, 10)]
    public string[] sentences;
}
