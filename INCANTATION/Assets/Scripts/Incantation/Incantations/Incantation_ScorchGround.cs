using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Scorch Ground", menuName = "Incantation/Scorch Ground")]
public class Incantation_ScorchGround : Incantation
{
    public GameObject scorch;

    private Transform player;

    public override void Cast()
    {
        Initialize();

        Instantiate(scorch, player.position, Quaternion.Euler(0f, 0f, 0f));
    }

    public void Initialize()
    {
        player = PlayerManager.instance.player.transform;
    }

    
}
