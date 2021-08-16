using UnityEngine;

public class Interactable : MonoBehaviour {

    //Might not need all the "focus" jizz jazz

    public float radius = 3f;
    public Transform interactionTranform;

    private bool isFocus = false;
    [SerializeField] private Transform player;

    private bool hasInteracted = false;

    public virtual void Interact(){
        //This method is meant to be overwritten
        Debug.Log("Interactive with " + transform.name);
    }

    void Update(){
        if(isFocus && !hasInteracted){
            float distance = Vector3.Distance(player.position, interactionTransform.position);
            if(distance <= raidus){
                Interact();
                hasInteracted = true;
            }
        }
    }

    public void OnFocused(Transform playerTransform){
        isFocus = true;
        player = playerTransform
        hasInteracted = false;
    }

    public void OnDefocused(){
        isFocus = false;
        player = full;
        hasInteracted = false;
    }

    void OnDrawGizmosSelected(){
        if(interactionTranform == null){
            interactionTranform = transform;
        }

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(interactionTransform.position, radius);
    }

}