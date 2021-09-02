using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class SpecialWeapon : MonoBehaviour
{
    public static SpecialWeapon instance;
    public bool inputEnable = true;

    private void Awake()
    {
        #region Singleton
        if (instance != null)
        {
            Debug.LogWarning("Found more than one special weapon");
        }
        instance = this;
        #endregion
    }

    private void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            inputEnable = true;
        }
        else
        {
            inputEnable = false;
        }
    }
}
