using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghosting : MonoBehaviour
{
    public float delay;
    private float delayTimer;
    public GameObject ghost;
    public PlayerStats playerStats;
    public float movementSpeedThreshold;
    private static TrailRenderer trailRenderer;

    private static float effectTimer;

    private void Start()
    {
        delayTimer = delay;
        trailRenderer = transform.parent.Find("Ghosting").GetComponent<TrailRenderer>();
    }

    private void FixedUpdate()
    {
        if (effectTimer > 0f)
        {
            effectTimer -= Time.deltaTime;

            if (delayTimer > 0f)
            {
                delayTimer -= Time.deltaTime;
            }
            else
            {
                GameObject ghostInstance = Instantiate(ghost, PlayerManager.instance.player.transform.position, transform.rotation);
                ghostInstance.GetComponent<SpriteRenderer>().sprite = GetComponent<SpriteRenderer>().sprite;

                delayTimer = delay;
                Destroy(ghostInstance, 1f);
            }
        }
        else
        {
            trailRenderer.enabled = false;
        }
    }

    public static void SetTrail(float duration)
    {
        effectTimer += duration;
        trailRenderer.enabled = true;
    }
}
