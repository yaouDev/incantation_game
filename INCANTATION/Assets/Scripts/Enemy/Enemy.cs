using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyStats))]
public class Enemy : Interactable
{
    //!!!!!! DEPRECATED !!!!!!
    //if enemy is to be interacted with, this script needs to be fixed so that it can be instansiated

    CharacterStats myStats;

    private void Start()
    {
        myStats = GetComponent<EnemyStats>();
    }

    public override void Interact()
    {
        base.Interact();

        /*
        CharacterCombat playerCombat = playerManager.player.GetComponent<CharacterCombat>();
        if(playerCombat != null)
        {
            playerCombat.Attack(myStats);
        }*/
        //attack enemy
    }
}
