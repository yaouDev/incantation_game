using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Incantation Scroll", menuName = "Inventory/Consumable/Incantation Scroll")]
public class IncantationScroll : Consumable
{
    public Incantation incantation;

    public override void Use()
    {
        if(incantation == null)
        {
            Debug.Log("Empty Incantation Scroll!");
            return;
        }

        //add cool text popup
        if (!IncantationManager.instance.GetUnlockedIncantations().ContainsValue(incantation))
        {
            IncantationManager.instance.AddIncantation(incantation);
            GameManager.instance.TextPopUp("You learnt a new Incantation!", incantation.name);
            base.Use();
        }
        else if (IncantationManager.instance.GetUnlockedIncantations().ContainsValue(incantation))
        {
            GameManager.instance.SendMessageToChat("You already know that incantation", Message.MessageType.info);
        }
        else
        {
            Debug.Log("Incantation Scroll: Database does not contain that incantation");
        }
    }
}
