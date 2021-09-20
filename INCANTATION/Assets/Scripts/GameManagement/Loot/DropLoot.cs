using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropLoot : MonoBehaviour
{
    public GameObject itemPickup;

    public List<Item> drops = new List<Item>();

    private List<Item> commonLoot = new List<Item>();
    private List<Item> uncommonLoot = new List<Item>();
    private List<Item> rareLoot = new List<Item>();
    private List<Item> mythicLoot = new List<Item>();
    private List<Item> legendaryLoot = new List<Item>();

    private Dictionary<Rarity, List<Item>> nullCheck = new Dictionary<Rarity, List<Item>>();

    private void Start()
    {
        foreach (Item i in drops)
        {
            switch (i.rarity)
            {
                case Rarity.common:
                    commonLoot.Add(i);
                    print(commonLoot);
                    break;
                case Rarity.uncommon:
                    uncommonLoot.Add(i);
                    break;
                case Rarity.rare:
                    rareLoot.Add(i);
                    break;
                case Rarity.mythic:
                    mythicLoot.Add(i);
                    break;
                case Rarity.legendary:
                    legendaryLoot.Add(i);
                    break;
                case Rarity.special:
                    legendaryLoot.Add(i);
                    Debug.Log("Special... nice..!");
                    break;
                default:
                    break;
            }
        }

        AddToNullCheck(Rarity.common, commonLoot);
        AddToNullCheck(Rarity.uncommon, uncommonLoot);
        AddToNullCheck(Rarity.rare, rareLoot);
        AddToNullCheck(Rarity.mythic, mythicLoot);
        AddToNullCheck(Rarity.legendary, legendaryLoot);
    }

    public Item Drop(Vector3 location)
    {
        if (nullCheck.Keys.Count <= 0f)
        {
            Debug.LogWarning("No loot set for " + gameObject.name);
            return null;
        }

        Rarity rarity;

        do
        {
            rarity = LootCalculator.instance.Calculcate();
        }
        while (!nullCheck.ContainsKey(rarity));

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

    private Item FindLoot(List<Item> array)
    {
        int index = Random.Range(0, array.Count);
        return array[index];
    }

    private void AddToNullCheck(Rarity rarity, List<Item> table)
    {
        if (table.Count > 0)
        {
            nullCheck.Add(rarity, table);
        }
    }
}
