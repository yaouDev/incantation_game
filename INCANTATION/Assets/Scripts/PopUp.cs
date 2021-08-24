using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class PopUp : MonoBehaviour
{
    public GameObject target;

    [SerializeField] private Vector3 startSize;
    public Vector3 targetSize;

    public bool useDuration = true;
    private bool manualActivation;
    [SerializeField] private float duration;
    [HideInInspector] public float timeRemaining;

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

        //USE DURATION
        if (timeRemaining > 0f && useDuration)
        {
            gameObject.transform.localScale += (targetSize - gameObject.transform.localScale) * 0.1f;
            timeRemaining -= Time.deltaTime;
        }
        else if(useDuration)
        {
            //VVV spaghetti
            if (gameObject.transform.localScale != startSize)
            {
                gameObject.transform.localScale += (startSize - gameObject.transform.localScale) * 0.1f;
            }
            else
            {
                //VVV turns the object off if its the startSize
                gameObject.SetActive(false);
            }
        }

        //MANUAL ACTIVATION
        if (manualActivation && gameObject.transform.localScale != targetSize && !useDuration)
        {
            gameObject.transform.localScale += (targetSize - gameObject.transform.localScale) * 0.1f;
        }
        else if(!manualActivation && gameObject.transform.localScale != startSize && !useDuration)
        {
            gameObject.transform.localScale += (startSize - gameObject.transform.localScale) * 0.1f;
        }
    }

    public void Pop()
    {
        if (useDuration)
        {
            timeRemaining = duration;
        }
        else
        {
            manualActivation = true;
        }
    }

    public void Return()
    {
        manualActivation = false;
    }
}