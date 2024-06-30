using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;
using System;

public abstract class PlayerActivatable : MonoBehaviour
{
    public bool canRepeat = false;
    public bool shouldActivateOnCollision = false;

    protected float DebounceInterval = 1f;
    protected bool isDebouncing = false;
    private float startTime = 0.0f;
    private bool hasRan = false;

    [HideInInspector]
    public bool canBeActivated = true;
 
    public virtual void OnColliding()
    {
        if (shouldActivateOnCollision) {
            if (!isDebouncing)
            {                
                Activate();                
            }
        }
    }

    public void Activate()
    {
        if (canBeActivated && (!hasRan || canRepeat)) {
            hasRan = true;
            StartCoroutine(HandleActiveState());
        }
    }

    protected virtual bool IsActivated() {
        return (Time.time - startTime) < DebounceInterval;
    }

    public virtual bool IsAvailable() {
        if (!canBeActivated) {
            return false;
        }
        if (hasRan && !canRepeat) {
            return false;
        }
        return !IsActivated();
    }

    protected abstract void OnActivate();

    IEnumerator HandleActiveState() {
        isDebouncing = true;
        OnActivate();
        startTime = Time.time;
        yield return new WaitWhile(IsActivated);
        isDebouncing = false;
    }

    protected void ResetRunFlag() {
        hasRan = false;
    }
    
}
