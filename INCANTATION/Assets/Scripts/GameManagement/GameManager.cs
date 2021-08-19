using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    #region Singleton

    public static GameManager instance;

    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogWarning("More than one game managers found");
        }

        instance = this;
    }

    #endregion

    //public bool isInputEnabled = true;

    public int maxMessages = 25;

    public GameObject chatPanel;
    public GameObject mainPanel;
    public GameObject textObject;
    public InputField chatBox;
    public Incantation incantation;

    public Color playerColor;
    public Color infoColor;
    public Color lootInfoColor;

    [SerializeField]
    private List<Message> messages = new List<Message>();

    void Start()
    {
        
    }


    void Update()
    {
        if(chatBox.text != "")
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                SendMessageToChat(chatBox.text, Message.MessageType.playerInput);
                chatBox.text = "";
                chatBox.gameObject.SetActive(false);
                incantation.checkMessages();
                mainPanel.SetActive(true);
                StartCoroutine(DisableObjectAfterDuration(mainPanel, 2f));
            }
        }
        else
        {
            if(!chatBox.isFocused && Input.GetKeyDown(KeyCode.Return))
            {
                chatBox.gameObject.SetActive(true);
                chatBox.ActivateInputField();
            } 
        }

        if (!chatBox.isFocused)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SendMessageToChat("Spacebar boiiii", Message.MessageType.lootInfo);
            }
        }
    }

    public void SendMessageToChat(string text, Message.MessageType messageType)
    {
        if(messages.Count >= maxMessages)
        {
            Destroy(messages[0].textObject.gameObject);
            messages.Remove(messages[0]);
            //Debug.Log("space");
        }

        Message newMessage = new Message();

        newMessage.text = text;

        GameObject newText = Instantiate(textObject, chatPanel.transform);

        newMessage.textObject = newText.GetComponent<Text>();

        newMessage.textObject.text = newMessage.text;
        newMessage.textObject.color = MessageTypeColor(messageType);

        messages.Add(newMessage);
    }

    private Color MessageTypeColor(Message.MessageType messageType)
    {
        Color color = infoColor;

        switch (messageType)
        {
            case Message.MessageType.playerInput:
                color = playerColor;
                break;
            case Message.MessageType.lootInfo:
                color = lootInfoColor;
                break;
            default:
                break;
        }

        return color;
    }

    public Message GetLatestMessage()
    {
        return messages[messages.Count - 1];
    }

    public IEnumerator EnableObjectAfterDuration(GameObject go, float duration)
    {
        float normalizedTime = 0f;
        while (normalizedTime <= 1f)
        {
            normalizedTime += Time.deltaTime / duration;
            yield return null;
        }

        go.SetActive(true);
    }

    public IEnumerator DisableObjectAfterDuration(GameObject go, float duration)
    {
        float normalizedTime = 0f;
        while (normalizedTime <= 1f)
        {
            normalizedTime += Time.deltaTime / duration;
            yield return null;
        }

        go.SetActive(false);
    }
}

[System.Serializable]
public class Message
{
    public string text;
    public Text textObject;
    public MessageType messageType;

    public enum MessageType
    {
        playerInput,
        info,
        lootInfo
    }
}
