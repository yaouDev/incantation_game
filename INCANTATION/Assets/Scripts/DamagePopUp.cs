using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DamagePopUp : MonoBehaviour
{
    private TMP_Text[] popUps;

    public float duration = 2f;
    public float raiseFactor = 0.01f;
    public float fadeOut = 0.98f;
    public bool selfDestruct;
    public GameObject followTarget;
    public Vector3 originalOffset = new Vector3(0f, 0f, 0f);

    private void Start()
    {
        popUps = gameObject.GetComponentsInChildren<TMP_Text>();

        foreach (TMP_Text text in popUps)
        {
            if(followTarget != null)
            {
                text.GetComponent<UIFollowGameObject>().target = followTarget;
            }
            text.text = "";
            text.enabled = false;
            SetPosition(text);
        }
    }

    public void Pop(Color color, int damage)
    {
        TMP_Text popper = null;

        for (int i = 0; i < popUps.Length; i++)
        {
            if (!popUps[i].enabled)
            {
                popper = popUps[i];
                popUps[i].enabled = true;
                break;
            }

            //if all popups are in use, recycle the first one
            if (i == popUps.Length - 1 && popUps[i].enabled)
            {
                popper = popUps[0];
                SetPosition(popper);
                Debug.Log("Damage Pop Up recycled on " + gameObject.name);
            }
        }

        if (popper == null)
        {
            Debug.Log("Popper was null on " + gameObject.transform.parent.name);
        }

        popper.text = "-" + damage;
        popper.color = color;
        popper.outlineColor = new Color(color.r * 0.1f, color.g * 0.1f, color.b * 0.1f);

        if (selfDestruct)
        {
            Destroy(gameObject, duration);
        }
    }

    private void FixedUpdate()
    {
        foreach (TMP_Text t in popUps)
        {
            UIFollowGameObject follow = t.GetComponent<UIFollowGameObject>();

            t.color = new Color(t.color.r, t.color.g, t.color.b, t.color.a * fadeOut);
            follow.offset = new Vector3(follow.offset.x, follow.offset.y + raiseFactor);

            if (t.color.a <= 0.1f)
            {
                t.enabled = false;
                SetPosition(t);
            }
        }
    }

    private void SetPosition(TMP_Text text)
    {
        //reset position with randoms
        text.GetComponent<UIFollowGameObject>().offset = new Vector3(originalOffset.x + Random.Range(-1f, 1f), originalOffset.y + Random.Range(0, 0.5f));
    }
}
