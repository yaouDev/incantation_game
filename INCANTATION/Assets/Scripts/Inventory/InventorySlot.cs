using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Image icon;
    public Button removeButton;
    public GameObject hover;
    public Text nameText;
    public Text descText;

    [SerializeField] private float timeToShowInfo = 1f;
    private CancellationTokenSource cts = null;

    [Header("ReadOnly")]
    public SlotType slotType;
    Item item;

    private void FixedUpdate()
    {
        /*
        if (EventSystem.current.IsPointerOverGameObject())
        {
            removeButton.enabled = true;
        }
        else
        {
            removeButton.enabled = false;
        }*/
        /*
        if (EventSystem.current.IsPointerOverGameObject() && EventSystem.current.gameObject == gameObject)
        {
            if (item != null)
            {
                showTimer -= Time.deltaTime;

                if (showTimer <= 0f)
                {
                    HoverBox();
                }
            }
        }
        else
        {
            showTimer = timeToShowInfo;
            hover.SetActive(false);
        }*/
    }

    public void AddItem(Item newItem)
    {
        if (!newItem.isDefaultItem)
        {
            item = newItem;

            icon.sprite = item.icon;
            icon.enabled = true;
            removeButton.interactable = true;
        }
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
        if (item != null)
        {
            if (slotType == SlotType.equipment)
            {
                //unequip
                Equipment equipment = (Equipment)item;
                equipment.UnequipToInventory();
            }
            else
            {
                item.Use();
            }
        }
    }

    private async Task HoverBox(float duration, CancellationToken token)
    {
        var end = Time.time + duration;
        while (Time.time < end)
        {
            if (token.IsCancellationRequested || item == null)
            {
                return;
            }

            await Task.Yield();
        }

        hover.SetActive(true);
        SetHoverText(item.name.GetLocalizedString(), item.description.GetLocalizedString());
    }

    private void SetHoverText(string name, string desc)
    {
        if (nameText != null && descText != null)
        {
            nameText.text = name;
            descText.text = desc;

            if (item.faction != null)
            {
                nameText.color = item.faction.color;
                descText.color = item.faction.color;
            }
        }
        else
        {
            Debug.LogWarning("Not sufficient info on item " + item.name.GetLocalizedString());
        }
    }

    async void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        if (item != null)
        {
            cts = new CancellationTokenSource();
            CancellationToken token = cts.Token;

            await HoverBox(timeToShowInfo, token);
        }


    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (cts != null && !cts.IsCancellationRequested)
        {
            cts.Cancel();
            cts.Dispose();
        }
        DisableHover();
    }

    public void DisableHover()
    {
        hover.SetActive(false);
    }
}

public enum SlotType
{
    inventory, equipment
}
