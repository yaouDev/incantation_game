using UnityEngine;
using UnityEngine.UI;

public class Interactable : MonoBehaviour
{
    public float radius = 3f;
    public Transform interactionTransform;

    [SerializeField] protected Transform player;
    public PlayerManager playerManager;

    public Text interactTextObject;
    public string interactText = "INTERACT";

    //private bool hasInteracted = false;


    public virtual void Interact()
    {
        //This method is meant to be overwritten
        Debug.Log("Interacted with " + transform.name);
    }

    private void Start()
    {
        InitializeInteractable();
    }

    protected void InitializeInteractable()
    {
        playerManager = PlayerManager.instance;
        player = playerManager.player.transform;

        if (interactTextObject != null)
        {
            interactTextObject.text = "PRESS " + "<size=20><color=yellow>E</color></size>" + " TO " + interactText;
            interactTextObject.enabled = false;
        }
    }

    void Update()
    {
        //poor performance???

        //currently picks up all items
        float distance = Vector3.Distance(player.position, interactionTransform.position);
        if (distance <= radius)
        {

            if(interactTextObject != null)
            {
                interactTextObject.enabled = true;
            }

            if (Input.GetButtonDown("Interact") && !GameManager.instance.chatBox.isFocused)
            {
                Interact();
                //hasInteracted = true;
            }
        }
        else
        {
            if (interactTextObject != null)
            {
                interactTextObject.enabled = false;
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        if (interactionTransform == null)
        {
            interactionTransform = transform;
        }

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(interactionTransform.position, radius);
    }

}