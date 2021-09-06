using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Incantation_DropTable : Incantation
{
    public override void Cast()
    {
        LootCalculator.instance.TestDrop();
    }
}
