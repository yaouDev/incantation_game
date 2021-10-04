using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetHeadBehindChest : MonoBehaviour
{
    public Animator playerAnimator;
    public GameObject chest;

    private SpriteRenderer sr;
    private SpriteRenderer chestr;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        chestr = chest.GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (playerAnimator.GetFloat("Vertical") > 0.01f)
        {
            sr.sortingOrder = chestr.sortingOrder - 1;
        }
        else if (playerAnimator.GetFloat("Vertical") <= 0.01f && sr.sortingOrder < chestr.sortingOrder)
        {
            sr.sortingOrder = chestr.sortingOrder + 1;
        }
    }
}
