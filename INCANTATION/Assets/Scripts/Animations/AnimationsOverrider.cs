using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationsOverrider : MonoBehaviour
{
    [SerializeField] private Animator anim;

    public void SetAnimation(AnimatorOverrideController aoc)
    {
        anim.runtimeAnimatorController = aoc;
    }
}
