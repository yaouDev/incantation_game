using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// make it so that unequipping an item doesnt require an empty slot if the swapped item is of the same type
/// unequipping with full inventory should be impossible
/// </summary>

public class Inventory : MonoBehaviour
{

    public Equipment[] equipment;
    public Item[] inventoryItems;

    public InventoryUI inventoryUI;

    public int filledSlots;
    public int latestIndex;

    public static Inventory instance;

    public int space = 20;
    public bool isFull;

    public delegate void OnItemChanged();
    public OnItemChanged onItemChangedCallback;

    void Awake()
    {

        #region Singleton
        if (instance != null)
        {
            Debug.LogWarning("More than one instance of inventory!");
            return;
        }

        instance = this;
        #endregion

        equipment = new Equipment[inventoryUI.equipmentSlots.Length];
        inventoryItems = new Item[inventoryUI.inventorySlots.Length];
    }

    public bool Add(Item item)
    {
        //unable to equip with full inventory?
        if (isFull)
        {
            Debug.Log("Inventory is full.");
            return false;
        }

        if (!item.isDefaultItem)
        {
            int invSpace;
            for (invSpace = 0; invSpace < inventoryItems.Length; invSpace++)
            {
                if (inventoryItems[invSpace] == null)
                {
                    inventoryItems[invSpace] = item;
                    filledSlots++;
                    if(filledSlots >= space)
                    {
                        isFull = true;
                    }
                    break;
                }
            }

            if (onItemChangedCallback != null)
            {
                onItemChangedCallback.Invoke();
            }
        }

        return true;
    }

    public void Remove(Item item)
    {
        for (int i = 0; i < inventoryItems.Length; i++)
        {
            if (inventoryItems[i] == item)
            {
                inventoryItems[i] = null;
                filledSlots--;
                if(filledSlots < space)
                {
                    isFull = false;
                }
                break;
            }
        }

        if (onItemChangedCallback != null)
        {
            onItemChangedCallback.Invoke();
        }
    }

    public int GetIndex(Item[] array, Item item)
    {
        for (int i = 0; i < array.Length; i++)
        {
            if(array[i] == item)
            {
                return i;
            }
        }

        Debug.Log("Item not in array");
        return 0;
    }
}