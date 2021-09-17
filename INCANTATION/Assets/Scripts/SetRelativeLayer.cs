using UnityEngine;

public class SetRelativeLayer : MonoBehaviour
{
    //make this take a renderer array

    [SerializeField] private int sortingOrderBase = 5000;
    private SpriteRenderer sr;
    [SerializeField] private int offset = 0;
    [SerializeField] private bool runOnlyOnce = false;
    [SerializeField] private bool isParticleSystem;

    private ParticleSystemRenderer pr;

    private void Awake()
    {
        sr = gameObject.GetComponent<SpriteRenderer>();
        if (isParticleSystem)
        {
            pr = gameObject.GetComponent<ParticleSystemRenderer>();
        }
    }

    private void LateUpdate()
    {
        if (isParticleSystem)
        {
            pr.sortingOrder = (int)(sortingOrderBase - transform.position.y - offset);
        }
        else
        {
            sr.sortingOrder = (int)(sortingOrderBase - transform.position.y - offset);
        }
        if (runOnlyOnce)
        {
            Destroy(this);
        }
    }

}
