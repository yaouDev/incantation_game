using UnityEngine;
using UnityEngine.UI;

public class EquipmentInterfaceSlot : MonoBehaviour
{
    //make all icons be enabled from the beginning and equal to defaultIcon

    public Image icon;
    public Sprite defaultIcon;

    public EquipmentManager equipmentManager;

    Equipment item;

    public void AddItem(Equipment newItem)
    {
        item = newItem;

        icon.sprite = item.icon;
        icon.enabled = true;
    }

    public void ClearSlot()
    {
        item = null;

        icon.sprite = defaultIcon;
        //icon.sprite = null;
        //icon.enabled = false;
    }

    public void OnClick()
    {
        if (item != null)
        {
            print(item.name + " was clicked");
            //equipmentManager.replace = false;
            equipmentManager.Unequip((int)item.equipSlot);
        }
    }
}
