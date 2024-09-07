using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class UseInventoryItem : UseInventoryItemBase
{
    void Awake() {
        SetTarget(gameObject);
    }

}
