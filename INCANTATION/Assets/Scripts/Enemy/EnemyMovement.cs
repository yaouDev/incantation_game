using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public bool isAggressive;
    public bool avoidsPlayer;
    public bool isMobile;
    private Rigidbody2D rb;
    private GameObject target;
    private EnemyStats stats;

    //Destination
    [Header("Passive Options")]
    private Vector2 destination;
    private bool enroute;
    public float stayAtPosTime = 2f;
    private float stayTimer;
    public float timerRadius = 15f;

    public Vector2 walkArea = new Vector2(2f, 2f);
    public Vector2 minimumWalkDistance = new Vector2(3f, 3f);
    private Vector2 negativeWalkArea;
    private Vector2 startPos;

    private Transform player;

    private void Start()
    {
        target = PlayerManager.instance.player;
        stats = GetComponent<EnemyStats>();
        rb = GetComponent<Rigidbody2D>();

        //ensures that it doesn't walk outside of the designated area
        walkArea -= minimumWalkDistance;
        negativeWalkArea = -walkArea;

        startPos = transform.position;

        CalculateDestination();
        enroute = true;

        player = PlayerManager.instance.player.transform;
    }

    private void FixedUpdate()
    {
        if (isMobile)
        {
            if (enroute)
            {
                Move(destination);
            }
        }

        if (avoidsPlayer)
        {
            if (isAggressive)
            {
                isAggressive = false;
            }

            float x = NormalizeValue(transform.position.x - player.position.x);
            float y = NormalizeValue(transform.position.y - player.position.y);

            Vector2 direction = new Vector2(x, y);

            print(direction);

            destination = (Vector2)transform.position + direction;
        }

        //VVV if enemy is close enough to its destination and what radius that is
        if (isAggressive)
        {
            destination = player.position;
        }
        
        if (!isAggressive && !avoidsPlayer)
        {
            float enterDistance = destination.sqrMagnitude - transform.position.sqrMagnitude;
            if (enterDistance <= timerRadius && enterDistance >= 0 || enterDistance > -timerRadius && enterDistance < 0)
            {
                if (enroute)
                {
                    enroute = false;
                    stayTimer = stayAtPosTime;
                    CalculateDestination();
                }
            }

            if (stayTimer > 0f)
            {
                stayTimer -= Time.deltaTime;
            }
            else
            {
                enroute = true;
            }
        }
    }

    private void CalculateDestination()
    {
        //destination to something
        float x = Random.Range(startPos.x + negativeWalkArea.x, startPos.x + walkArea.x);
        float y = Random.Range(startPos.y + negativeWalkArea.y, startPos.y + walkArea.y);

        x = x < 0 ? x -= minimumWalkDistance.x : x += minimumWalkDistance.x;
        y = y < 0 ? y -= minimumWalkDistance.y : y += minimumWalkDistance.y;

        Vector2 newDestination = new Vector2(x, y);

        destination = newDestination;
    }

    public int NormalizeValue(float value)
    {
        if(value == 0)
        {
            return 0;
        }

        return value < 0 ? -1 : 1;
    }

    private void Move(Vector2 targetPos)
    {
        float damp = (targetPos.normalized.sqrMagnitude - transform.position.normalized.sqrMagnitude) + 1f;
        transform.position = Vector2.MoveTowards(transform.position, targetPos, stats.movementSpeed.GetValue() / 500f * damp);

        /*
        Vector2 movement;

        movement.x = NormalizeValue(targetPos.x - transform.position.x);
        movement.y = NormalizeValue(targetPos.y - transform.position.y);

        print(movement);
        int moveSpeed = stats.movementSpeed.GetValue();

        //Movement
        if (movement.x != 0 && movement.y != 0)
        {
            rb.MovePosition(rb.position + movement * (moveSpeed / 10) * 0.75f * Time.fixedDeltaTime);
        }
        else
        {
            rb.MovePosition(rb.position + movement * (moveSpeed / 10) * Time.fixedDeltaTime);
        }*/
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, destination);
    }
}
