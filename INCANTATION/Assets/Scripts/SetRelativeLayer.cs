using UnityEngine;

public class SetRelativeLayer : MonoBehaviour
{
    public Transform occlusionTrigger;
    public Transform occlusionPoint;
    private SpriteRenderer sr;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if(occlusionTrigger.position.y > occlusionPoint.position.y)
        {
            sr.sortingLayerName = "InfrontPlayer";
        }
        else
        {
            sr.sortingLayerName = "BehindPlayer";
        }
    }
}
