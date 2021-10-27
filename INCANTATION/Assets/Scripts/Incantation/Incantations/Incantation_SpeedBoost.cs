using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Speed Boost", menuName = "Incantation/Speed Boost")]
public class Incantation_SpeedBoost : Incantation
{
    private PlayerStats stats;
    public int modifier = 100;
    public float duration = 5f;
    private Ghosting ghost;

    public override async void Cast()
    {
        Initialize();

        ghost.StartCoroutine(ghost.Trail(duration));
        await stats.StatBoost(stats.movementSpeed, modifier, duration);
        //stats.StartCoroutine(stats.StatBoost(stats.movementSpeed, modifier, duration));
    }

    public void Initialize()
    {
        ghost = PlayerManager.instance.player.GetComponentInChildren<Ghosting>();
        stats = PlayerManager.instance.player.GetComponent<PlayerStats>();
    }
}
