using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

[RequireComponent(typeof(Animator))]
public class SimpleAnimationTrigger : PlayerActivatable
{
    public GameObject target;
    
    private Animator animator;
    private bool isPlaying = false;

    private int currentAnimationValidationId = 0;

    void Start()
    {
        if (target == null) {
            target = gameObject;
        }
        animator = target.GetComponent<Animator>();
        if (animator) {
            animator.speed = 0f;
        }
    }

    override protected void OnActivate()
    {
        Animate();
    }

    public void Animate() {
        if (animator) {
            animator.speed = 1f;
            isPlaying = true;
            StartCoroutine(WaitForAnimationToEnd());
        }
    }

    float GetCurrentTime() {
        return animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
    }

    IEnumerator WaitForAnimationToEnd() {
        currentAnimationValidationId ++;
        int validationId = currentAnimationValidationId;
        while (GetCurrentTime() >= 1.0f) {
            if (currentAnimationValidationId != validationId) break;
            yield return null;
        }
        while (GetCurrentTime() < 1.0f) {
            if (currentAnimationValidationId != validationId) break;
            yield return null;
        }
        isPlaying = false;
    }

    override protected bool IsActivated() 
    {
        return isPlaying;
    }

}
