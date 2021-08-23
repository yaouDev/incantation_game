using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIFollowGameObject : MonoBehaviour
{
    public GameObject target;
    public Vector3 offset = new Vector3(0f, 0f, 0f);
    private Camera cam;

    private void Start()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        Vector3 relativePosition = cam.WorldToScreenPoint(target.transform.position + offset);
        transform.position = relativePosition;
    }
}
