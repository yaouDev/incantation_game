using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DisableClickOnHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        PlayerCombatManager.instance.clickIsAttack = false;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        PlayerCombatManager.instance.clickIsAttack = true;
    }
}
