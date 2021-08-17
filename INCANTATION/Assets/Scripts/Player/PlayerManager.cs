using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    #region Singleton

    public static PlayerManager instance;

    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogWarning("More than once player manager!");
        }

        instance = this;
    }

    #endregion

    public GameObject player;
}
