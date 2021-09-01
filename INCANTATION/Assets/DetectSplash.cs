using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectSplash : MonoBehaviour
{
    private ParticleSystem particles;

    private void Start()
    {
        particles = GetComponent<ParticleSystem>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        particles.Play();
        //splash audio
    }
}
