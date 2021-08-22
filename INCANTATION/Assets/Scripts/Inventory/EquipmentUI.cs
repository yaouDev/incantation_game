using UnityEngine;

public class EquipmentUI : MonoBehaviour
{
    public Transform itemsParent;

    [ReadOnly] public EquipmentInterfaceSlot[] slots;

    void Start()
    {
        //slots = new EquipmentInterfaceSlot[System.Enum.GetNames(typeof(EquipmentSlot)).Length];
        slots = itemsParent.GetComponentsInChildren<EquipmentInterfaceSlot>();
    }
}
