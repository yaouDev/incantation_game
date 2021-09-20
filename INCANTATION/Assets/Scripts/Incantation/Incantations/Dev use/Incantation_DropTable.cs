using UnityEngine;

[CreateAssetMenu(fileName = "Roll Table", menuName = "Incantation/Roll Table")]
public class Incantation_DropTable : Incantation
{
    public override void Cast()
    {
        LootCalculator.instance.TestDrop();
    }
}
