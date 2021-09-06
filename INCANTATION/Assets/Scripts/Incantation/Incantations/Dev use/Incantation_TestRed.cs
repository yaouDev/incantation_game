using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Incantation_TestRed : Incantation
{
    private Camera cam;

    public override void Cast()
    {
        cam = Camera.main;

        cam.backgroundColor = Color.red;
        print("background turned red!");
    }
}
