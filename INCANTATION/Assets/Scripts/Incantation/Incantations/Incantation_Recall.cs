using UnityEngine;

public class Incantation_Recall : Incantation
{
    [SerializeField] private GameObject recallPoint;

    public override void Cast()
    {
        PlacePoint();
    }

    private void PlacePoint()
    {
        Instantiate(recallPoint, PlayerManager.instance.player.transform.position, PlayerManager.instance.player.transform.rotation);
    }
}
