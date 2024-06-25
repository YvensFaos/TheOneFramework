using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

[RequireComponent(typeof(Animator))]
public class AnimationToggle : PlayerActivatable
{
    public enum ToggleType
    {
        Looped,
        Once,
        Random
    }

    public GameObject target;
    public string[] animationStates;

    public ToggleType toggleMode = ToggleType.Looped;
    
    private Animator animator;

    private bool isPlaying = false;

    private Queue<string> animationQueue = new Queue<string>();

    private int currentAnimationValidationId = 0;

    void Start()
    {
        if (target == null) {
            target = gameObject;
        }
        animator = target.GetComponent<Animator>();
        animationQueue = GetAnimationQueue();
    }

    private Queue<string> GetAnimationQueue() {
        var result = new Queue<string>();
        switch (toggleMode) {
            case ToggleType.Looped:
            case ToggleType.Once:
                foreach (string state in animationStates) {
                    result.Enqueue(state);
                }
                break;
            case ToggleType.Random:
                var shuffledStates = new List<string>(animationStates);

                var rng = new System.Random();
                int count = shuffledStates.Count;
                while (count > 1) {
                    count--;
                    int randomIndex = rng.Next(count + 1);
                    var value = shuffledStates[randomIndex];
                    shuffledStates[randomIndex] = shuffledStates[count];
                    shuffledStates[count] = value;
                }
                foreach (string state in shuffledStates) {
                    result.Enqueue(state);
                }
                break;
        }
        return result;
    }

    override protected void OnActivate()
    {
        ToggleAnimationStates();
    }

    private string GetNextAnimationState() {
        if (animationQueue.Count < 1) {
            if (toggleMode != ToggleType.Once) {
                animationQueue = GetAnimationQueue();
            } else {
                return null;
            }
        }
        return animationQueue.Dequeue();
    }

    public void ToggleAnimationStates() {
        if (animator) {
            var animationState = GetNextAnimationState();
            if (animationState != null) {
                animator.Play(animationState);
                isPlaying = true;
                StartCoroutine(WaitForAnimationToEnd());
            }
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
