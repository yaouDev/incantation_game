using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestWeapon : WeaponAttack
{

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.isInputEnabled && Input.GetButtonDown("Fire1"))
        {
            Debug.Log("Click!");
        }
    }
}
