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
    public Text playerIncantationText;
    public float playerTextDuration = 2f;
    public GameObject incantationPanel;

    public Text popUpMainText;
    public Text popUpSubText;
    public GameObject popUpParent;

    public Color playerColor;
    public Color infoColor;
    public Color lootInfoColor;

    [SerializeField]
    private List<Message> messages = new List<Message>();

    void Start()
    {
        playerIncantationText.color = playerColor;
        playerIncantationText.text = "";
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
    }

    public void SendMessageToChat(string text, Message.MessageType messageType)
    {
        if(messages.Count >= maxMessages)
        {
            Destroy(messages[0].textObject.gameObject);
            messages.Remove(messages[0]);
        }

        Message newMessage = new Message();

        newMessage.text = text;

        GameObject newText = Instantiate(textObject, chatPanel.transform);
        if(messageType == Message.MessageType.playerInput && incantation.currentIncantations.Contains(newMessage.text))
        {
            //Sets the overhead text to playerInput and remove the text after a while if it's available
            //if you type faster than the coroutine, the message will disappear with the previous timer
            playerIncantationText.text = newMessage.text.ToUpper() + "!";
            incantationPanel.SetActive(true);
            StartCoroutine(SetTextEmptyAfterDuration(playerIncantationText, playerTextDuration));
            StartCoroutine(DisableObjectAfterDuration(incantationPanel, playerTextDuration));
        }

        newMessage.textObject = newText.GetComponent<Text>();

        newMessage.textObject.text = newMessage.text;
        newMessage.textObject.color = MessageTypeColor(messageType);

        mainPanel.SetActive(true);
        StartCoroutine(DisableObjectAfterDuration(mainPanel, 2f));

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

    public IEnumerator SetTextEmptyAfterDuration(Text text, float duration)
    {
        float normalizedTime = 0f;
        while (normalizedTime <= 1f)
        {
            normalizedTime += Time.deltaTime / duration;
            yield return null;
        }

        text.text = "";
    }

    public void TextPopUp(string mainText, string subText)
    {
        StartCoroutine(TextPopUpIE(mainText, subText));
    }

    private IEnumerator TextPopUpIE(string mainText, string subText)
    {
        if (popUpParent.GetComponent<PopUp>().timeRemaining > 0f)
        {
            float normalizedTime = 0f;
            while (normalizedTime <= 1f)
            {
                normalizedTime += Time.deltaTime / (popUpParent.GetComponent<PopUp>().timeRemaining + 2f);
                print(normalizedTime);
                yield return null;
            }
        }
        popUpMainText.text = mainText.ToUpper();
        popUpSubText.text = subText.ToUpper();
        popUpParent.SetActive(true);
        popUpParent.GetComponent<PopUp>().Pop();
    }

    private IEnumerator WaitDuration(float duration)
    {
        float normalizedTime = 0f;
        while (normalizedTime <= 1f)
        {
            normalizedTime += Time.deltaTime / duration;
            yield return null;
        }
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
