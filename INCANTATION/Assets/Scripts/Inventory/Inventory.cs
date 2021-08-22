using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// make it so that unequipping an item doesnt require an empty slot if the swapped item is of the same type
/// unequipping with full inventory should be impossible
/// </summary>

public class Inventory : MonoBehaviour
{

    public Item[] items;
    private int filledSlots;
    public int latestIndex;

    public static Inventory instance;

    public int space = 20;
    public bool isFull { get; private set; }

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

        items = new Item[space];
    }

    public bool Add(Item item)
    {
        if (isFull)
        {
            Debug.Log("Inventory is full.");
            return false;
        }

        if (!item.isDefaultItem)
        {
            int invSpace;
            for (invSpace = 0; invSpace < items.Length; invSpace++)
            {
                if (items[invSpace] == null)
                {
                    items[invSpace] = item;
                    filledSlots++;
                    latestIndex = invSpace;
                    if (filledSlots >= space)
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

    public void AddOnIndexOf(int index, Item item)
    {
        items[index] = item;
    }

    public void Remove(Item item)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] == item)
            {
                items[i] = null;
                filledSlots--;
                //latestIndex = i;
                isFull = false;
                break;
            }
        }

        if (onItemChangedCallback != null)
        {
            onItemChangedCallback.Invoke();
        }
    }

    public int GetFilledSlots()
    {
        return filledSlots;
    }
}