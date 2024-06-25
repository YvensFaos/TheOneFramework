using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseInventoryItem : UseInventoryItemOnTarget
{
    void Awake() {
        SetTarget(gameObject);
    }

}
