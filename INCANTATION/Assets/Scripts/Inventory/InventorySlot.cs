using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public Image icon;
    public Button removeButton;

    Item item;

    /*
     * broken.. fix
    private void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            removeButton.enabled = true;
        }
        else
        {
            removeButton.enabled = false;
        }
    }*/

    public void AddItem(Item newItem)
    {
        item = newItem;

        icon.sprite = item.icon;
        icon.enabled = true;
        removeButton.interactable = true;
    }

    public void ClearSlot()
    {
        item = null;

        icon.sprite = null;
        icon.enabled = false;
        removeButton.interactable = false;
    }

    public void OnEquipmentClicked()
    {
        if (item != null)
        {
            print(item.name + " was clicked");
            //equipmentManager.replace = false;
            Equipment newItem = (Equipment)item;
            EquipmentManager.instance.Unequip((int)newItem.equipSlot);
        }
    }

    public void OnRemoveButton()
    {
        Inventory.instance.Remove(item);
    }

    public void UseItem()
    {
        if(item != null)
        {
            item.Use();
        }
    }
}
