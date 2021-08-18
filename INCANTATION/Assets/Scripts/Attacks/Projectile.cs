using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    //change to a constant for no chargingVVV
    [HideInInspector] public float projectileVelocity;
    private Rigidbody2D rb;

    //should be awake??
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        //VVchange to be deleted on impact or duration
        Destroy(gameObject, 4f);
    }

    private void FixedUpdate()
    {
        rb.velocity = transform.up * projectileVelocity;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }
}
