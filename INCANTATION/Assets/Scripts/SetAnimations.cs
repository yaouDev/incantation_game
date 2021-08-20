using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetAnimations : MonoBehaviour
{
    public AnimatorOverrideController[] overrideControllers;
    [SerializeField] private AnimationsOverrider overrider;

    public void Set(int value)
    {
        overrider.SetAnimation(overrideControllers[value]);
    }
}
