using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteFlash : MonoBehaviour
{
    public float flashDuration = 0.1f;
    public SpriteRenderer originalSpriteRenderer;
    private SpriteMask sm;

    private void Start()
    {
        gameObject.SetActive(false);
        sm = GetComponent<SpriteMask>();
    }

    public void Play()
    {
        gameObject.SetActive(true);
        sm.sprite = originalSpriteRenderer.sprite;
        StartCoroutine(Activate(flashDuration));
    }

    private IEnumerator Activate(float duration)
    {
        originalSpriteRenderer.enabled = false;
        
        float normalizedTime = 0f;
        while(normalizedTime <= 1f)
        {
            normalizedTime += Time.deltaTime / duration;
            yield return null;
        }

        gameObject.SetActive(false);
        originalSpriteRenderer.enabled = true;
    }
}
