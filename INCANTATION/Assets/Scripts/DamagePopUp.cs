using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamagePopUp : MonoBehaviour
{
    public Text text;
    public float duration = 2f;
    public UIFollowGameObject UIFollow;

    //public float fade = 0.001f;

    private void Awake()
    {
        UIFollow.offset = new Vector3(UIFollow.offset.x + Random.Range(-1f, 1f), UIFollow.offset.y + Random.Range(0, 0.5f));
        Destroy(gameObject, duration);
    }

    private void Update()
    {
        text.color = new Color(text.color.r, text.color.g, text.color.b, text.color.a - 0.002f);
        UIFollow.offset = new Vector3(UIFollow.offset.x, UIFollow.offset.y + 0.003f);
    }
}
