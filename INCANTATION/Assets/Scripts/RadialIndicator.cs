using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadialIndicator : MonoBehaviour
{
    public float radius;

    private Vector3 startScale = new Vector3(0f, 0f);
    private Vector3 endScale;

    private bool reached;
    private SpriteRenderer sr;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        gameObject.transform.localScale = startScale;
        endScale = new Vector2(radius, radius);
    }

    private void Update()
    {
        if(gameObject.transform.localScale.sqrMagnitude >= endScale.sqrMagnitude)
        {
            reached = true;
        }


        if (!reached)
        {
            gameObject.transform.localScale += (gameObject.transform.localScale - endScale) * 0.2f;
        }
        else
        {
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.g, sr.color.a - 0.01f);
        }

        if(sr.color.a <= 0f)
        {
            Destroy(gameObject);
        }
    }
}
