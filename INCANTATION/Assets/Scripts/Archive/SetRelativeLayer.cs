using System.Collections.Generic;
using UnityEngine;

public class SetRelativeLayer : MonoBehaviour
{
    //make this take a renderer array

    [SerializeField] private int sortingOrderBase = 5000;
    public List<Renderer> renderers;
    [SerializeField] private int offset = 0;
    [SerializeField] private bool runOnlyOnce = false;
    [SerializeField] private bool hasParticles;

    private ParticleSystemRenderer pr;

    private void Awake()
    {
        this.enabled = false;
        Debug.LogWarning("Set Layer present on " + gameObject.transform.parent.name);

        if (hasParticles)
        {
            pr = GetComponent<ParticleSystemRenderer>();
        }

        if (renderers.Count <= 0 && pr == null)
        {
            Debug.LogWarning("No renderers found for " + gameObject.name);
        }
    }

    private void LateUpdate()
    {
        foreach(Renderer r in renderers)
        {
            r.sortingOrder = (int)(sortingOrderBase - transform.position.y - offset);
        }

        if (hasParticles)
        {
            pr.sortingOrder = (int)(sortingOrderBase - transform.position.y - offset);
        }

        if (runOnlyOnce)
        {
            Destroy(this);
        }
    }

}
