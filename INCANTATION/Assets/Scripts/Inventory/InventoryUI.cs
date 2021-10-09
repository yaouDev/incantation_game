using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public Transform itemsParent;
    public GameObject inventoryUI;

    Inventory inventory;

    public InventorySlot[] equipmentSlots;
    public InventorySlot[] inventorySlots;

    // Start is called before the first frame update
    void Start()
    {
        inventory = Inventory.instance;
        inventory.onItemChangedCallback += UpdateUI;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Inventory") && !GameManager.instance.chatBox.isFocused)
        {
            inventoryUI.SetActive(!inventoryUI.activeSelf);

            PlayerCombatManager.instance.clickIsAttack = true;

            //VVV cursor related
            if (GameManager.instance.integrateCursor)
            {
                Cursor.visible = !Cursor.visible;
            }
        }
    }

    void UpdateUI()
    {
        for (int i = 0; i < inventory.inventoryItems.Length; i++)
        {
            if(inventory.inventoryItems[i] != null)
            {
                inventorySlots[i].AddItem(inventory.inventoryItems[i]);
            }
            else
            {
                inventorySlots[i].ClearSlot();
            }
        }

        if(inventory.filledSlots >= inventory.space)
        {
            inventory.isFull = true;
        }
        else
        {
            inventory.isFull = false;
        }

        for (int i = 0; i < inventory.equipment.Length; i++)
        {
            if (inventory.equipment[i] != null)
            {
                equipmentSlots[i].AddItem(inventory.equipment[i]);
            }
            else
            {
                equipmentSlots[i].ClearSlot();
            }
        }
    }
}
