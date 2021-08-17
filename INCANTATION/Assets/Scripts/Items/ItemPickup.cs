using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class ItemPickup : Interactable{

    public Item item;
    public SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = item.sprite;
    }

    public override void Interact(){
        base.Interact();

        PickUp();
    }

    public void PickUp(){
        Debug.Log("Picking up " + item.name);
        bool wasPickedUp = Inventory.instance.Add(item);

        if(wasPickedUp){
            Destroy(gameObject);
        }
    }

}