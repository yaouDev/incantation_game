using UnityEngine;


public class Item : ScriptableObject{

    new public string name = "New Item";
    public Sprite icon = null;
    public bool isDefaultItem = false;

    //[CreateAssetMenu(filename = "New Item", menuName = "Inventory/Item")]

}