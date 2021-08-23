using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Consumable : Item
{
    public override void Use()
    {
        base.Use();

        //do stuff
        RemoveFromInventory();
    }
}
