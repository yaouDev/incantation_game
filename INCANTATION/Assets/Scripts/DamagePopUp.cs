using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamagePopUp : MonoBehaviour
{
    public Text text;
    public float duration = 2f;

    public float fade = 0.001f;

    private void Awake()
    {
        Destroy(gameObject, duration);
    }

    private void Update()
    {
        text.color = new Color(text.color.r, text.color.g, text.color.b, text.color.a - 0.002f);
    }
}
