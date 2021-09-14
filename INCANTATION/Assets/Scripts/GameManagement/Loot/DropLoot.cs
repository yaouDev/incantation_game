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

    private Dictionary<Rarity, Item[]> drops = new Dictionary<Rarity, Item[]>();

    private void Start()
    {
        AddToDrops(Rarity.common, commonLoot);
        AddToDrops(Rarity.uncommon, uncommonLoot);
        AddToDrops(Rarity.rare, rareLoot);
        AddToDrops(Rarity.mythic, mythicLoot);
        AddToDrops(Rarity.legendary, legendaryLoot);
    }

    public Item Drop(Vector3 location)
    {
        if (commonLoot.Length == 0 && uncommonLoot.Length == 0 && rareLoot.Length == 0 && mythicLoot.Length == 0 && legendaryLoot.Length == 0)
        {
            Debug.LogWarning("No loot set for " + gameObject.name);
            return null;
        }

        Rarity rarity;

        do
        {
            rarity = LootCalculator.instance.Calculcate();
        }
        while (!drops.ContainsKey(rarity));

        ItemPickup lootInstance = Instantiate(itemPickup, location, itemPickup.transform.localRotation).GetComponent<ItemPickup>();

        switch (rarity)
        {
            case Rarity.common:
                lootInstance.item = FindLoot(commonLoot);
                break;
            case Rarity.uncommon:
                lootInstance.item = FindLoot(uncommonLoot);
                break;
            case Rarity.rare:
                lootInstance.item = FindLoot(rareLoot);
                break;
            case Rarity.mythic:
                lootInstance.item = FindLoot(mythicLoot);
                break;
            case Rarity.legendary:
                lootInstance.item = FindLoot(legendaryLoot);
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
        int index = Random.Range(0, array.Length);
        return array[index];
    }

    private void AddToDrops(Rarity rarity, Item[] table)
    {
        if (table.Length > 0)
        {
            drops.Add(rarity, table);
        }
    }
}
