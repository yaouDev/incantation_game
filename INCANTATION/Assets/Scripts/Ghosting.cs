using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Ghosting : MonoBehaviour
{
    //public float delay;
    //public GameObject ghost;
    //public PlayerStats playerStats;
    //public float movementSpeedThreshold;
    private TrailRenderer trailRenderer;
    //private Sprite sprite;

    //[Header("Read Only")]
    //public float effectTimer;

    private void Start()
    {
        trailRenderer = GetComponent<TrailRenderer>();
        //sprite = GetComponent<SpriteRenderer>().sprite;
    }

    public async Task SetTrail(float duration)
    {
        trailRenderer.enabled = true;

        var end = Time.time + duration;
        while (Time.time < end)
        {
            await Task.Yield();
        }

        trailRenderer.enabled = false;
    }

    public IEnumerator Trail(float duration)
    {
        trailRenderer.enabled = true;

        var end = Time.time + duration;
        while(Time.time < end)
        {
            yield return null;
        }

        trailRenderer.enabled = false;
    }
}
