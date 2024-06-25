using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Yarn.Unity;

public class UnityEventTrigger : PlayerActivatable 
{
    public UnityEvent targetEvent;

    override protected void OnActivate()
    {        
        if (targetEvent != null)
        {
            targetEvent.Invoke();
        }
    }

}
