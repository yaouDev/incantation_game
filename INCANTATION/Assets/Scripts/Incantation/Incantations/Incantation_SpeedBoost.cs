using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Speed Boost", menuName = "Incantation/Speed Boost")]
public class Incantation_SpeedBoost : Incantation
{
    private PlayerStats stats;
    public int modifier = 100;
    public float duration = 5f;

    public override void Cast()
    {
        Initialize();

        stats.StartCoroutine(stats.StatBoost(stats.movementSpeed, modifier, duration));
        Ghosting.SetTrail(duration);
    }

    public void Initialize()
    {
        stats = PlayerManager.instance.player.GetComponent<PlayerStats>();
    }
}
