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

    private DialogManager dm;

    private void Start()
    {
        InitializeInteractable();
        dm = DialogManager.instance;
    }

    private void FixedUpdate()
    {
        if (timer > 0f)
        {
            timer -= Time.deltaTime;
        }
    }

    public void Talk()
    {
        if (!dm.isChoosing && dialog.Length > 0)
        {
            if (dm.isConversing)
            {
                dm.DisplayNextSentence();
            }
            else
            {
                dm.StartDialog(dialog);
            }
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
