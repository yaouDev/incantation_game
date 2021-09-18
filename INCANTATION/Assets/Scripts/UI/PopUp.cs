using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class PopUp : MonoBehaviour
{
    public GameObject target;

    public Vector3 startSize;
    public Vector3 targetSize;

    public bool useDuration = true;
    private bool manualActivation;
    [SerializeField] private float duration;
    [HideInInspector] public float timeRemaining;

    public bool targetSelf;

    void Awake()
    {
        if (targetSelf)
        {
            target = gameObject;
        }
        target.transform.localScale = startSize;
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
            target.transform.localScale += (targetSize - target.transform.localScale) * 0.1f;
            timeRemaining -= Time.deltaTime;
            target.SetActive(true);
        }
        else if(useDuration && timeRemaining <= 0f)
        {
            //VVV spaghetti
            if (target.transform.localScale != startSize)
            {
                target.transform.localScale += (startSize - target.transform.localScale) * 0.1f;
            }
            else
            {
                //VVV turns the object off if its the startSize
                target.SetActive(false);
            }
        }

        //MANUAL ACTIVATION
        if (manualActivation && target.transform.localScale != targetSize && !useDuration)
        {
            target.transform.localScale += (targetSize - target.transform.localScale) * 0.1f;
        }
        else if(!manualActivation && target.transform.localScale != startSize && !useDuration)
        {
            target.transform.localScale += (startSize - target.transform.localScale) * 0.1f;
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