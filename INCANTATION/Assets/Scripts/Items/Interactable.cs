using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Interactable : MonoBehaviour
{
    public float radius = 3f;
    public Transform interactionTransform;

    [SerializeField] protected Transform player;
    public PlayerManager playerManager;

    private static GameObject closest;

    public Text interactTextObject;
    public string interactText = "INTERACT";

    public bool isInteractable = true;

    public virtual void Interact()
    {
        //This method is meant to be overwritten
        Debug.Log("Interacted with " + transform.name);
    }

    private void Start()
    {
        InitializeInteractable();

        radius *= 2;
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
        //float distance = (player.transform.position - interactionTransform.position).sqrMagnitude;
        float distance = Vector3.Distance(player.transform.position, interactionTransform.position);
        if(distance <= radius && closest == null)
        {
            closest = gameObject;
        }
        else if (distance > radius && isInteractable)
        {
            if (closest == gameObject)
            {
                closest = null;
            }
        }

        if (closest == gameObject && isInteractable)
        {

            if(interactTextObject != null)
            {
                interactTextObject.enabled = true;
            }

            if (Input.GetButtonDown("Interact") && !GameManager.instance.chatBox.isFocused)
            {
                Interact();

                if(TryGetComponent(out NPCInteractable npc))
                {
                    return;
                }

                closest = null;
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