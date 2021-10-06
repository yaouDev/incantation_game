using UnityEngine;
using UnityEngine.Localization;

[CreateAssetMenu(fileName = "Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject{

    new public LocalizedString name;
    public LocalizedString description;
    public Sprite icon = null;
    public Sprite sprite = null;
    public bool isDefaultItem = false;
    public Rarity rarity = Rarity.common;

    public virtual void Use()
    {
        //Something happens here

        Debug.Log("Using " + name.GetLocalizedString());
    }

    public void RemoveFromInventory()
    {
        Inventory.instance.Remove(this);
    }

    public void SetRarity(Rarity newRarity)
    {
        rarity = newRarity;
    }
}

public enum Rarity{
    //Subject to change names
    common, uncommon, rare, mythic, legendary, special
}