using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetRelativeLayer : MonoBehaviour
{
    private GameObject player;
    private SpriteRenderer sr;
    private bool isInfront;
    public float offsetY;

    private void Start()
    {
        player = PlayerManager.instance.player;
        sr = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if(player.transform.Find("Footing").transform.position.y > gameObject.transform.position.y + offsetY && !isInfront)
        {
            sr.sortingOrder = player.GetComponent<SpriteRenderer>().sortingOrder + 1;
            isInfront = true;
        }
        else if (player.transform.position.y <= gameObject.transform.position.y + offsetY)
        {
            sr.sortingOrder = player.GetComponent<SpriteRenderer>().sortingOrder - 1;
            isInfront = false;
        }
    }
}
