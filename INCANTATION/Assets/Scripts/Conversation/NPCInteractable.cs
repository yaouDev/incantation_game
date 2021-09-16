using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCInteractable : Interactable
{
    [Header("NPC-related")]
    [SerializeField] private NPC thisCharacter;

    public Dialog[] dialog;

    [SerializeField] private float interactInterval = 0.5f;
    private float timer;

    private void Start()
    {
        InitializeInteractable();
    }

    private void FixedUpdate()
    {
        if(timer > 0f)
        {
            timer -= Time.deltaTime;
        }
    }

    public void Talk()
    {
        if (DialogManager.instance.isConversing)
        {
            DialogManager.instance.DisplayNextSentence();
        }
        else
        {
            DialogManager.instance.StartDialog(dialog);
        }
    }

    public override void Interact()
    {
        base.Interact();

        if (timer <= 0f)
        {
            Talk();
            timer = interactInterval;
        }
    }
}
