using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropLoot : MonoBehaviour
{
    public GameObject itemPickup;

    public Item[] commonLoot = new Item[0];
    public Item[] uncommonLoot = new Item[0];
    public Item[] rareLoot = new Item[0];
    public Item[] mythicLoot = new Item[0];
    public Item[] legendaryLoot = new Item[0];

    public Item Drop(Vector3 location)
    {
        if(commonLoot.Length == 0 && uncommonLoot.Length == 0 && rareLoot.Length == 0 && mythicLoot.Length == 0 && legendaryLoot.Length == 0)
        {
            Debug.LogWarning("No loot set for " + gameObject.name);
            return null;
        }

        Rarity rarity = LootCalculator.instance.Calculcate();

        ItemPickup lootInstance = Instantiate(itemPickup, location, itemPickup.transform.localRotation).GetComponent<ItemPickup>();

        switch (rarity)
        {
            case Rarity.common:
                if (commonLoot.Length > 0)
                {
                    lootInstance.item = FindLoot(commonLoot);
                }
                else
                {
                    lootInstance.item = FindLoot(legendaryLoot);
                }
                break;
            case Rarity.uncommon:
                if (uncommonLoot.Length > 0)
                {
                    lootInstance.item = FindLoot(uncommonLoot);
                }
                else
                {
                    lootInstance.item = FindLoot(commonLoot);
                }
                break;
            case Rarity.rare:
                if (rareLoot.Length > 0)
                {
                    lootInstance.item = FindLoot(rareLoot);
                }
                else
                {
                    lootInstance.item = FindLoot(uncommonLoot);
                }
                break;
            case Rarity.mythic:
                if (mythicLoot.Length > 0)
                {
                    lootInstance.item = FindLoot(mythicLoot);
                }
                else
                {
                    lootInstance.item = FindLoot(rareLoot);
                }
                break;
            case Rarity.legendary:
                if (legendaryLoot.Length > 0)
                {
                    lootInstance.item = FindLoot(legendaryLoot);
                }
                else
                {
                    lootInstance.item = FindLoot(mythicLoot);
                }
                break;
            case Rarity.special:
                lootInstance.item = FindLoot(commonLoot);
                Debug.Log("Special... nice..!");
                break;
            default:
                break;
        }

        return lootInstance.item;
    }

    private Item FindLoot(Item[] array)
    {
        int index = Random.Range(0, array.Length - 1);
        return array[index];
    }
}
