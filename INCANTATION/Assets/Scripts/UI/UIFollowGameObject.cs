using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIFollowGameObject : MonoBehaviour
{
    public GameObject target;
    public Vector3 offset = new Vector3(0f, 0f, 0f);
    private Camera cam;
    [SerializeField] private bool isStaticOnScreen;

    private void Start()
    {
        cam = Camera.main;

        if(target == null)
        {
            Debug.Log("The target of " + gameObject.name + " is null!");
        }
    }

    private void Update()
    {
        if(target != null)
        {
            Vector3 relativePosition = cam.WorldToScreenPoint(target.transform.position + offset);
            transform.position = relativePosition;
        }

        if (isStaticOnScreen)
        {
            Destroy(this);
        }
    }
}
