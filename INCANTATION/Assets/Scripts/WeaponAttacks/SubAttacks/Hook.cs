using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hook : Projectile
{
    //Currently collides with enemy projectiles!

    private GameObject hookedObject;
    public bool fetch;
    public float killDistance = 3f;
    public float destroyDistance = 100f;

    public float fetchDivider = 20f;
    public float grabDivider = 10f;

    private GameObject player;
    private Vector2 distance;
    private float distanceCheck1;
    private float distanceCheck2;
    [SerializeField] private float stuckTimer = 1f;

    private LineRenderer lr;

    private void Start()
    {
        player = PlayerManager.instance.player;
        lr = GetComponent<LineRenderer>();
    }

    private void FixedUpdate()
    {
        if(hookedObject != null)
        {
            if (fetch)
            {
                distance = player.transform.position - gameObject.transform.position;
                hookedObject.transform.position = transform.position;
                rb.MovePosition(rb.position + distance * (projectileVelocity / fetchDivider) * Time.fixedDeltaTime);
            }
            else
            {
                rb.velocity = Vector2.zero;
                distance = gameObject.transform.position - player.transform.position;
                player.GetComponent<Rigidbody2D>().MovePosition(player.GetComponent<Rigidbody2D>().position + (distance * Time.deltaTime));
            }

            Debug.Log(stuckTimer);

            distanceCheck1 = (gameObject.transform.position - player.transform.position).sqrMagnitude;

            if (distanceCheck2 - distanceCheck1 <= 0.5f)
            {
                stuckTimer -= Time.deltaTime;
            }

            distanceCheck2 = distanceCheck1;

            float destroyDistance = Vector2.Distance(player.transform.position, gameObject.transform.position);
            if(destroyDistance < killDistance || stuckTimer <= 0f)
            {
                Destroy(gameObject);
            }
        }
        else
        {
            rb.velocity = transform.up * projectileVelocity;
        }

        //LineRenderer
        lr.SetPosition(0, PlayerCombatManager.instance.weapon.transform.position);
        lr.SetPosition(1, gameObject.transform.position);

        //Destroy if line is too far
        float relativeDistanceX = lr.GetPosition(1).x - lr.GetPosition(0).x;
        float relativeDistanceY = lr.GetPosition(1).y - lr.GetPosition(0).y;
        if (relativeDistanceX > destroyDistance || relativeDistanceY > destroyDistance)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(hookedObject == null)
        {
            hookedObject = collision.gameObject;
        }

        if(hookedObject.TryGetComponent(out EnemyStats enemy))
        {
            fetch = true;
            GetComponent<CapsuleCollider2D>().enabled = false;
        }
    }
}
