using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Incantation : MonoBehaviour
{
    private List<string> incantations = new List<string>();
    private Message message = new Message();
    public GameManager gameManager;

    //delete
    public Camera background;

    // Start is called before the first frame update
    void Start()
    {
        AddIncantation("add red");
        AddIncantation("add blue");
        AddIncantation("remove red");
        AddIncantation("remove blue");
        AddIncantation("die");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void checkMessages()
    {
        message = gameManager.GetLatestMessage();
        print(message.text);

        if (message.messageType == Message.MessageType.playerInput && incantations.Contains(message.text))
        {
            ExecuteIncantation(message.text);
        }
        else if (message.messageType == Message.MessageType.playerInput && !incantations.Contains(message.text))
        {
            Debug.Log("Not unlocked yet");
        }
    }

    private void ExecuteIncantation(string incantation)
    {
        switch (incantation)
        {
            case "red":
                background.backgroundColor = Color.red;
                break;
            case "blue":
                background.backgroundColor = Color.blue;
                break;
            case "add red":
                AddIncantation("red");
                break;
            case "add blue":
                AddIncantation("blue");
                break;
            case "remove red":
                RemoveIncantation("red");
                break;
            case "remove blue":
                RemoveIncantation("blue");
                break;
            case "die":
                gameObject.GetComponent<PlayerState>().TakeDamage(10);
                break;
            default:
                break;

        }
    }

    public void AddIncantation(string incantation)
    {
        incantations.Add(incantation);
        Debug.Log("Added " + incantation);
    }

    public void RemoveIncantation(string incantation)
    {
        incantations.Remove(incantation);
        Debug.Log("Removed " + incantation);
    }

    public List<string> GetIncantations()
    {
        return incantations;
    }
}
