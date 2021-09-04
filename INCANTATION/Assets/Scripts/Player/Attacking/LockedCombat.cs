using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockedCombat : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerCombat.instance.attackPoint.transform.position != PlayerCombat.instance.weapon.transform.position)
        {
            PlayerCombat.instance.attackPoint.transform.position = PlayerCombat.instance.weapon.transform.position;
        }
        PlayerCombat.instance.attackPoint.transform.localPosition = new Vector3(PlayerCombat.instance.attackPoint.transform.localPosition.x, (PlayerCombat.instance.attackRange * 0.85f) - PlayerCombat.instance.baseAttackRange);
    }
}
