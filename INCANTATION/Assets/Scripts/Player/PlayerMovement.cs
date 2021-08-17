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
        /*
        if (EventSystem.current.IsPointerOverGameObject())
        {
            Stop();
            return;
        }*/

        //Input

        if (!gameManager.chatBox.isFocused)
        {
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");
        }
        else
        {
            Stop();
        }

        if (movement != Vector2.zero)
        {
            animator.SetFloat("Horizontal", movement.x);
            animator.SetFloat("Vertical", movement.y);
        }

        animator.SetFloat("Speed", movement.sqrMagnitude);
    }

    private void Stop()
    {
        movement.x = 0f;
        movement.y = 0f;
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
}
