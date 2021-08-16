using UnityEngine;

public class Interactable : MonoBehaviour {

    //Might not need all the "focus" jizz jazz

    public float radius = 3f;
    public Transform interactionTransform;

    //private bool isFocus = false;
    [SerializeField] private Transform player;

    private bool hasInteracted = false;

    public virtual void Interact(){
        //This method is meant to be overwritten
        Debug.Log("Interacted with " + transform.name);
    }
    /*
    void Update(){
        if(!hasInteracted){
            float distance = Vector3.Distance(player.position, interactionTransform.position);
            if(distance <= radius){
                Interact();
                hasInteracted = true;
            }
        }
    }*/

    /*
    public void OnFocused(Transform playerTransform){
        isFocus = true;
        player = playerTransform;
        hasInteracted = false;
    }

    public void OnDefocused(){
        isFocus = false;
        player = null;
        hasInteracted = false;
    }*/

    void OnDrawGizmosSelected(){
        if(interactionTransform == null){
            interactionTransform = transform;
        }

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(interactionTransform.position, radius);
    }

}