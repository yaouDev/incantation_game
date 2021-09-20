using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Test Red", menuName = "Incantation/Test Red")]
public class Incantation_TestRed : Incantation
{
    private Camera cam;

    public override void Cast()
    {
        cam = Camera.main;

        cam.backgroundColor = Color.red;
    }
}
