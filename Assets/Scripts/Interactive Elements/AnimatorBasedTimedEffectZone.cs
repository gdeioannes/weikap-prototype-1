using UnityEngine;
using System.Collections;

public class AnimatorBasedTimedEffectZone : BaseTimedEffectZone {

    [SerializeField] Animator animator;
    [SerializeField] string isActiveAnimatorProperty;

    protected override void ActivateElements()
    {
        animator.SetBool(isActiveAnimatorProperty, true);
    }

    protected override void DeactivateElements()
    {
        animator.SetBool(isActiveAnimatorProperty, false);
    }
}
