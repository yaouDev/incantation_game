using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization;

public class DialogManager : MonoBehaviour
{
    public static DialogManager instance;

    public Text nameText;
    public Text dialogText;
    public Image speakerImage;

    public Animator animator;
    public Animator choiceAnimator;

    private Dialog[] dialogs;
    private Queue<LocalizedString> sentences;

    private PlayerManager pm;
    [ReadOnly] public bool isConversing;

    private int currentDialog;
    [ReadOnly] public bool isChoosing;

    public Choice choice;

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
        sentences = new Queue<LocalizedString>();
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
            if (currentDialog < dialogs.Length - 1 && !dialogs[currentDialog].isEnd)
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

        if (dialogs[currentDialog].hasChoice)
        {
            Choice();
        }
        else
        {
            choiceAnimator.SetBool("IsOpen", false);
        }

        LocalizedString sentence = sentences.Dequeue();
        StopCoroutine(TypeSentence(sentence));
        StartCoroutine(TypeSentence(sentence));
    }

    private void SetSentences()
    {
        //set up the speaker
        if (dialogs[currentDialog].speaker == null)
        {
            dialogs[currentDialog].speaker = PlayerManager.instance.character;
        }

        Character speaker = dialogs[currentDialog].speaker;

        nameText.text = speaker.name.GetLocalizedString();
        speakerImage.sprite = speaker.dialogGFX;
        sentences.Clear();

        foreach (LocalizedString sentence in dialogs[currentDialog].sentences)
        {
            sentences.Enqueue(sentence);
        }
    }

    IEnumerator TypeSentence(LocalizedString sentence)
    {
        dialogText.text = "";

        string typedSentence = sentence.GetLocalizedString();

        foreach (char letter in typedSentence.ToCharArray())
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
        currentDialog = 0;

        Debug.Log("End of conversation");
    }

    private void PlayVoiceline(Character character)
    {
        //audio
        AudioClip clip = character.characterClassification.GetVoiceline();
        GetComponent<AudioSource>().PlayOneShot(clip);
    }

    private void Choice()
    {
        choiceAnimator.SetBool("IsOpen", true);

        isChoosing = true;

        string[] choiceText = new string[dialogs[currentDialog].choices.Length];

        for (int i = 0; i < dialogs[currentDialog].choices.Length; i++)
        {
            choiceText[i] = dialogs[currentDialog].choices[i].GetLocalizedString();
        }

        choice.SetChoice(choiceText);
    }

    public void Choose(int selection)
    {
        if (isChoosing)
        {
            Dialog dialog = dialogs[currentDialog];
            int jump = dialog.jumps[selection];

            if (jump > 0)
            {
                currentDialog = jump;
                SetSentences();
            }

            DisplayNextSentence();

            isChoosing = false;
        }
    }
}
