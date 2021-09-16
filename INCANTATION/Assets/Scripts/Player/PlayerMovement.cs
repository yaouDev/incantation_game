using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 75f;

    public Rigidbody2D rb;
    public Animator animator;
    public float pickupRadius = 5f;

    private Camera cam;
    private Animator[] equipmentAnimators;

    public GameManager gameManager;

    private Vector2 movement;

    private bool frozen;

    void Start()
    {
        cam = Camera.main;
        equipmentAnimators = new Animator[gameManager.GetComponent<EquipmentManager>().GetEquipmentAnimators().Length];
        equipmentAnimators = gameManager.GetComponent<EquipmentManager>().GetEquipmentAnimators();
    }

    void Update()
    {
        /*
        if (EventSystem.current.IsPointerOverGameObject())
        {
            Stop();
            return;
        }*/

        if (frozen)
        {
            Stop();
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
            Stop();
        }

        if (movement != Vector2.zero)
        {
            animator.SetFloat("Horizontal", movement.x);
            animator.SetFloat("Vertical", movement.y);
            for (int i = 0; i < equipmentAnimators.Length; i++)
            {
                if(i == (int)EquipmentSlot.weapon)
                {
                    i++;
                }
                equipmentAnimators[i].SetFloat("Horizontal", movement.x);
                equipmentAnimators[i].SetFloat("Vertical", movement.y);
            }
        }

        animator.SetFloat("Speed", movement.sqrMagnitude);
        for (int i = 0; i < equipmentAnimators.Length; i++)
        {
            if (i == (int)EquipmentSlot.weapon)
            {
                i++;
            }
            equipmentAnimators[i].SetFloat("Speed", movement.sqrMagnitude);
        }
    }
    
    private void Stop()
    {
        movement.x = 0f;
        movement.y = 0f;
        animator.SetFloat("Speed", 0);
    }

    private void FixedUpdate()
    {
        //Movement
        if (movement.x != 0 && movement.y != 0)
        {
            rb.MovePosition(rb.position + movement * (moveSpeed/10) * 0.75f * Time.fixedDeltaTime);
        }
        else
        {
            rb.MovePosition(rb.position + movement * (moveSpeed/10) * Time.fixedDeltaTime);
        }
    }

    public Vector2 GetMovement()
    {
        return movement;
    }

    public void Freeze(bool freeze)
    {
        frozen = freeze;
    }
}
