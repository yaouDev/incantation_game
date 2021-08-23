using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Incantation Scroll", menuName = "Inventory/Consumable/Incantation Scroll")]
public class IncantationScroll : Consumable
{
    public string incantation;
    public override void Use()
    {
        //add cool text popup
        if (!Incantation.instance.GetPermanentIncantations().Contains(incantation))
        {
            Incantation.instance.AddIncantation(incantation);
            GameManager.instance.TextPopUp("You learnt a new Incantation!", incantation);
            base.Use();
        }
        else if (Incantation.instance.GetPermanentIncantations().Contains(incantation))
        {
            GameManager.instance.SendMessageToChat("You already know that incantation", Message.MessageType.info);
        }
        else
        {
            Debug.Log("Incantation Scroll: Database does not contain that incantation");
        }
    }
}
