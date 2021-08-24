using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    public float speed = 1f;
    public float originalSpeed;
    public bool isSpinning;

    [Header("Combat-related")]
    public bool isUsedForCombat;
    public float combatSpeed = 4f;
    private PopUp pop;

    private void Start()
    {
        originalSpeed = speed;
        if(GetComponent<PopUp>() != null)
        {
            pop = GetComponent<PopUp>();
            pop.target = gameObject;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isSpinning)
        {
            gameObject.transform.Rotate(new Vector3(gameObject.transform.rotation.x, gameObject.transform.rotation.y, speed));
        }

        if (isUsedForCombat)
        {
            //scale speed up down?
            if (Input.GetButton("Fire1"))
            {
                pop.Pop();
                speed = combatSpeed;
            }
            else
            {
                pop.Return();
                speed = originalSpeed;
            }
        }
        else
        {
            pop.Return();
        }
    }
}
