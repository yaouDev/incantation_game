using UnityEngine;

public class Interactable : MonoBehaviour
{
    public float radius = 3f;
    public Transform interactionTransform;

    [SerializeField] private Transform player;
    public PlayerManager playerManager;

    //private bool hasInteracted = false;


    public virtual void Interact()
    {
        //This method is meant to be overwritten
        Debug.Log("Interacted with " + transform.name);
    }

    private void Start()
    {
        playerManager = PlayerManager.instance;
        player = playerManager.player.transform;
    }

    void Update()
    {

        //currently picks up all items
        //if (!hasInteracted)
        //{
            float distance = Vector3.Distance(player.position, interactionTransform.position);
            if (distance <= radius && Input.GetButtonDown("Interact"))
            {
                Interact();
                //hasInteracted = true;
            }
        //}
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