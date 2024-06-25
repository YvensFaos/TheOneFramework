using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseInventoryItemOnTarget : UseInventoryItemBase
{
    [SerializeField] protected GameObject targetGameObject = null;

    void Awake() {
        SetTarget(targetGameObject);
    }

}
