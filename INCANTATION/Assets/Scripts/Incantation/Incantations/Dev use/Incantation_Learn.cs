using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Learn", menuName = "Incantation/Learn")]
public class Incantation_Learn : Incantation
{
    public Incantation toBeLearnt;

    public override void Cast()
    {
        if(toBeLearnt != null)
        {
            IncantationManager.instance.AddIncantation(toBeLearnt);
        }
    }
}
