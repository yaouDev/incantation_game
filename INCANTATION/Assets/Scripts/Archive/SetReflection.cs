using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetReflection : MonoBehaviour
{
    private SpriteRenderer sr;
    private SpriteRenderer reflectedRenderer;
    [SerializeField] private Vector3 offset;

    private void Awake()
    {
        reflectedRenderer = GetComponent<SpriteRenderer>();
        sr = transform.parent.GetComponent<SpriteRenderer>();

        transform.localPosition += offset;
    }

    private void Update()
    {
        reflectedRenderer.sprite = sr.sprite;
    }

}
