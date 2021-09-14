using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecallPoint : MonoBehaviour
{
    private List<Vector2> path = new List<Vector2>();

    private GameObject player;
    public Image visualTimer;
    private float startTimer;

    [SerializeField] private float recordInterval = 0.1f;
    private float recordTimer;
    [SerializeField] private float recallTimer = 5f;

    [SerializeField] private float recallInterval = 0f;
    private float returnTimer;

    private bool recall;

    private LineRenderer lr;
    private int playerPoint;

    void Start()
    {
        player = PlayerManager.instance.player;
        lr = GetComponent<LineRenderer>();
        lr.SetPosition(0, transform.position);
        startTimer = recallTimer;

        visualTimer.gameObject.GetComponent<UIFollowGameObject>().target = player;
    }

    void FixedUpdate()
    {
        if (recallTimer > 0f)
        {
            recallTimer -= Time.deltaTime;

            if (recordTimer > 0f)
            {
                recordTimer -= Time.deltaTime;
            }
            else
            {
                Record();

                recordTimer = recordInterval;
            }
        }

        if (!recall)
        {
            visualTimer.fillAmount = recallTimer / startTimer;
            lr.SetPosition(playerPoint + 1, player.transform.position);
        }

        if (recallTimer <= 0f)
        {
            if (recall)
            {
                if (returnTimer > 0f)
                {
                    returnTimer -= Time.deltaTime;
                }
                else
                {
                    Recall();
                    returnTimer = recallInterval;
                }
            }
            else
            {
                LockPlayer();
            }

            if (path.Count <= 0)
            {
                player.GetComponent<PlayerMovement>().Freeze(false);
                Destroy(gameObject);
            }
        }
    }

    private void Recall()
    {
        player.transform.position = Vector2.Lerp(player.transform.position, path[0], 10f);

        if (Vector2.Distance(player.transform.position, path[0]) < 0.1f)
        {
            path.Remove(path[0]);
            lr.positionCount--;
        }
    }

    private void LockPlayer()
    {
        player.GetComponent<PlayerMovement>().Freeze(true);
        path.Reverse();
        recall = true;
        returnTimer = recallInterval;
    }

    private void Record()
    {
        path.Add(player.transform.position);
        lr.positionCount++;
        lr.SetPosition(++playerPoint, player.transform.position);
    }
}
