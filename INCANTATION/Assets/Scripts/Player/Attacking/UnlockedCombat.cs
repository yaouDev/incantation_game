using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockedCombat : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        PlayerCombat.instance.attackPoint.localPosition = new Vector3(PlayerCombat.instance.lookDir.x, PlayerCombat.instance.lookDir.y);
    }
}
