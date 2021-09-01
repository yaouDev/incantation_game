using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectReflection : MonoBehaviour
{
    public GameObject reflection;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        print("enter");

        if (collision.gameObject.TryGetComponent<SpriteRenderer>(out SpriteRenderer sr))
        {
            if(collision.gameObject.GetComponentInChildren<SetReflection>() == null)
            {
                Instantiate(reflection, collision.gameObject.transform);
            }
        }
    }
}
