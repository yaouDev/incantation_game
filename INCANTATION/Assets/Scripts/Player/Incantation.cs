using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Incantation : MonoBehaviour
{
    public List<string> allIncantations = new List<string>()
    {
        "red",
        "blue",
        "die",
        "add blue",
        "add red",
        "ring",
        "spawn enemy",
        "remove red",
        "remove blue"
    };

    public List<string> currentIncantations = new List<string>();
    private Message message = new Message();
    public GameManager gameManager;
    public GameObject enemy;

    //delete
    public Camera background;

    // Start is called before the first frame update
    void Start()
    {
        AddIncantation("add blue");
        AddIncantation("remove red");
        AddIncantation("remove blue");
        AddIncantation("die");
        AddIncantation("ring");
        AddIncantation("spawn enemy");
        foreach (string str in allIncantations)
        {
            print(str);
        }
    }

    public void checkMessages()
    {
        message = gameManager.GetLatestMessage();
        print(message.text);

        if (message.messageType == Message.MessageType.playerInput && currentIncantations.Contains(message.text))
        {
            ExecuteIncantation(message.text);
        }
        else if (message.messageType == Message.MessageType.playerInput && !currentIncantations.Contains(message.text) && allIncantations.Contains(message.text))
        {
            Debug.Log("Not unlocked yet");
        }
        else if(message.messageType == Message.MessageType.playerInput && !currentIncantations.Contains(message.text) && !allIncantations.Contains(message.text))
        {
            Debug.Log("Not an incantation");
        }
    }

    //ADD/REMOVE INCANTATION
    public void AddIncantation(string incantation)
    {
        if (allIncantations.Contains(incantation))
        {
            currentIncantations.Add(incantation);
            Debug.Log("Added " + incantation);
        }
        else
        {
            Debug.Log("No such incantation in database");
        }
    }

    public void RemoveIncantation(string incantation)
    {
        if (currentIncantations.Contains(incantation))
        {
            currentIncantations.Remove(incantation);
            Debug.Log("Removed " + incantation);
        }
        else
        {
            Debug.Log("Tried to remove locked or non-existent incantation");
        }
    }

    private void ExecuteIncantation(string incantation)
    {
        if (currentIncantations.Contains(incantation))
        {
            switch (incantation)
            {
                case "add blue":
                    AddIncantation("blue");
                    break;
                case "red":
                    background.backgroundColor = Color.red;
                    break;
                case "blue":
                    background.backgroundColor = Color.blue;
                    break;
                case "remove red":
                    RemoveIncantation("red");
                    break;
                case "remove blue":
                    RemoveIncantation("blue");
                    break;
                case "die":
                    gameObject.GetComponent<PlayerStats>().TakeDamage(gameObject.GetComponent<PlayerStats>().maxHealth.GetValue());
                    break;
                case "ring":
                    RingOfDeath();
                    break;
                case "spawn enemy":
                    SpawnEnemy();
                    break;
                default:
                    break;

            }
        }
        else if(!currentIncantations.Contains(incantation) && allIncantations.Contains(incantation))
        {
            //Push to chatPanel?
            Debug.Log("Not unlocked yet!");
        }
        else
        {
            Debug.LogWarning("Tried to execute incantation not currently in the database");
        }
    }

    public List<string> GetIncantations()
    {
        return currentIncantations;
    }

    //-----INCANTATION METHODS-----
    private void RingOfDeath()
    {
        float radius = 5f;

        Collider2D[] colliderArray = Physics2D.OverlapCircleAll(transform.position, radius);
        foreach (Collider2D collider in colliderArray)
        {
            if (collider.TryGetComponent<EnemyStats>(out EnemyStats enemy))
            {
                enemy.TakeDamage(10);
            }
        }
    }

    private void SpawnEnemy()
    {
        Instantiate(enemy, new Vector3(transform.position.x, transform.position.y + 3), transform.rotation);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(transform.position, 5f);
    }
}
