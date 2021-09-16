using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
    public static DialogManager instance;

    public Text nameText;
    public Text dialogText;
    public Image speakerImage;

    public Animator animator;

    private Dialog[] dialogs;
    private Queue<string> sentences;

    private PlayerManager pm;
    [ReadOnly] public bool isConversing;

    private int currentDialog;

    private void Awake()
    {
        #region Singleton
        if (instance != null)
        {
            Debug.LogWarning("More than one dialog manager found!");
            return;
        }
        instance = this;
        #endregion
    }

    void Start()
    {
        sentences = new Queue<string>();
        pm = PlayerManager.instance;
    }

    public void StartDialog(Dialog[] dialog)
    {
        isConversing = true;
        dialogs = dialog;
        currentDialog = 0;

        animator.SetBool("IsOpen", true);

        //stop player
        pm.player.GetComponent<PlayerMovement>().Freeze(true);

        SetSentences();
        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        PlayVoiceline(dialogs[currentDialog].speaker);

        if (sentences.Count == 0)
        {
            if (currentDialog < dialogs.Length - 1)
            {
                currentDialog++;
                SetSentences();
            }
            else
            {
                EndDialog();
                return;
            }
        }

        string sentence = sentences.Dequeue();
        StopCoroutine(TypeSentence(sentence));
        StartCoroutine(TypeSentence(sentence));
    }

    private void SetSentences()
    {
        //set up the speaker
        if(dialogs[currentDialog].speaker == null)
        {
            dialogs[currentDialog].speaker = PlayerManager.instance.character;
        }

        Character speaker = dialogs[currentDialog].speaker;

        nameText.text = speaker.name;
        speakerImage.sprite = speaker.dialogGFX;
        sentences.Clear();

        foreach (string sentence in dialogs[currentDialog].sentences)
        {
            sentences.Enqueue(sentence);
        }
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogText.text = "";

        foreach (char letter in sentence.ToCharArray())
        {
            dialogText.text += letter;
            yield return null;
        }
    }
    private void EndDialog()
    {
        isConversing = false;
        pm.player.GetComponent<PlayerMovement>().Freeze(false);
        animator.SetBool("IsOpen", false);

        //reset manager

        Debug.Log("End of conversation");
    }

    private void PlayVoiceline(Character character)
    {
        //audio
        AudioClip clip = character.characterClassification.GetVoiceline();
        GetComponent<AudioSource>().PlayOneShot(clip);
    }
}
