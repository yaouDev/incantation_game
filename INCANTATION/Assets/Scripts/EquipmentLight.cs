using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class EquipmentLight : MonoBehaviour
{
    //change to private
    public Light2D[] lights;
    public Vector3[] originalPositions;


    private void Start()
    {
        int numSlots = System.Enum.GetNames(typeof(EquipmentSlot)).Length;

        lights = new Light2D[numSlots];
        originalPositions = new Vector3[numSlots];

        for (int i = 0; i < EquipmentManager.instance.equipmentObjects.Length; i++)
        {
            lights[i] = EquipmentManager.instance.equipmentObjects[i].GetComponentInChildren<Light2D>();
            lights[i].enabled = false;
        }
        

        /*
        for (int i = 0; i < numSlots; i++)
        {
            lights[i] = EquipmentManager.instance.equipmentObjects[i].GetComponentInChildren<Light2D>();
            originalPositions[i] = lights[i].gameObject.transform.localPosition;
        }*/

        EquipmentManager.instance.onEquipmentChanged += UpdateLights;
    }

    void UpdateLights(Equipment newItem, Equipment oldItem)
    {
        if (newItem != null)
        {
            int slotIndex = (int)newItem.equipSlot;

            lights[slotIndex].gameObject.transform.localPosition = originalPositions[slotIndex];

            if (newItem.turnOnLight)
            {
                lights[slotIndex].color = newItem.lightColor;
                lights[slotIndex].intensity = newItem.lightIntensity;
                lights[slotIndex].pointLightInnerRadius = newItem.lightInnerRange;
                lights[slotIndex].pointLightOuterRadius = newItem.lightOuterRange;
                lights[slotIndex].gameObject.transform.localPosition += newItem.lightOffset;
                lights[slotIndex].enabled = true;
            }
        }
        else
        {
            lights[(int)oldItem.equipSlot].enabled = false;
        }
        /*
        if (newItem != null)
        {
            if (EquipmentManager.instance.equipmentObjects[(int)newItem.equipSlot] == gameObject && newItem.turnOnLight)
            {
                lights.color = newItem.lightColor;
                lights.intensity = newItem.lightIntensity;
                lights.pointLightInnerRadius = newItem.lightInnerRange;
                lights.pointLightOuterRadius = newItem.lightOuterRange;
                lightObjects.transform.localPosition += newItem.lightOffset;
                lightObjects.SetActive(true);
            }
        }
        else
        {
            lightObjects.transform.localPosition = originalPosition;
            lightObjects.SetActive(false);
        }*/
    }
}
