using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class TextPopUp : MonoBehaviour
{
    //possible to add a list/queue that stores items until the timer is over and then pops that one

    public Text text;

    [SerializeField] private Vector3 startSize;
    public Vector3 targetSize;

    [SerializeField] private float duration;
    private float timeRemaining;

    private void Start()
    {

    }

    void Awake()
    {
        gameObject.transform.localScale = startSize;
    }

    void Update()
    {
        /*
        if (GameObject.FindGameObjectsWithTag("Menu").Length > 0)
        {
            timeRemaining = 0f;
        }*/

        if (timeRemaining > 0f)
        {
            gameObject.transform.localScale += (targetSize - gameObject.transform.localScale) * 0.1f;
            timeRemaining -= Time.deltaTime;
        }
        else
        {
            //VVV spaghetti
            if (gameObject.transform.localScale != new Vector3(0, 0, 0))
            {
                gameObject.transform.localScale += (startSize - gameObject.transform.localScale) * 0.1f;
            }
            else
            {
                //VVV turns the object off if its the startSize
                gameObject.SetActive(false);
            }
        }

    }

    public void PopUp()
    {
        timeRemaining = duration;
    }
}