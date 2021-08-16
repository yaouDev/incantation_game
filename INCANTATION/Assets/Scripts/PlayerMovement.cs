using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;

    public Rigidbody2D rb;
    public Animator animator;
    public float pickupRadius = 5f;

    private Camera cam;

    public GameManager gameManager;

    private Vector2 movement;

    public Interactable focus;

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        //Input

        if (!gameManager.chatBox.isFocused)
        {
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");
        }
        else
        {
            movement.x = 0f;
            movement.y = 0f;
        }

        if (movement != Vector2.zero)
        {
            animator.SetFloat("Horizontal", movement.x);
            animator.SetFloat("Vertical", movement.y);
        }

        animator.SetFloat("Speed", movement.sqrMagnitude);

        //Input right mouse
        /*
        if(Input.GetMouseButtonDown(1)){

            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if(Physics.Raycast(ray, out hit, 100)){
                Interactable interactable = hit.collider.GetComponent<Interactable>();

                if(interactable != null){
                    SetFocus(interactable);
                }
            }
        }*/

        //Input E BROKEN!!! RECOGNIZES PLAYER COLLIDER
        if (Input.GetButtonDown("Interact"))
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(gameObject.transform.position, pickupRadius);
            foreach (Collider2D collider in colliders)
            {
                if (collider.gameObject.TryGetComponent<ItemPickup>(out ItemPickup item))
                {
                    item.Interact();
                }
            }
        }

    }

    private void FixedUpdate()
    {
        //Movement
        if (movement.x != 0 && movement.y != 0)
        {
            rb.MovePosition(rb.position + movement * moveSpeed * 0.75f * Time.fixedDeltaTime);
        }
        else
        {
            rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
        }



    }

    /*
    private void SetFocus(Interactable newFocus){
        if(newFocus != focus){

            if(focus != null){
                focus.OnDefocused();
            }
            focus = newFocus;
        }

        focus = newFocus;
        newFocus.OnFocused(transform);
    }

    private void RemoveFocus(){

        if(focus != null){
            focus.OnDefocused();
        }
        focus = null;
    }*/
}
