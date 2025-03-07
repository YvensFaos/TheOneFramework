using UnityEngine.Events;
using UnityEngine;

public class OnItemUnityEventTrigger : PlayerActivatable
{
    public Inventory inventory;
    [SerializeField] InventoryItemDefinition item;
    [SerializeField] bool shouldRemoveFromInventory = true;

    public UnityEvent onItemCheckPasses;
    public UnityEvent onItemCheckFails;

    [System.Obsolete]
    void Start() {
        if (inventory == null) {
            inventory = FindFirstObjectByType<Inventory>();
        }
    }

    override protected void OnActivate()
    {
        if (item == null) {
            Debug.LogError("The UseInventoryItem(OnTarget) has no 'item' property set. The behaviour will not activate.");
        }
        if (inventory != null && item != null) {
            if (inventory.HasItem(item.name)) {
                if (shouldRemoveFromInventory) {
                    inventory.RemoveItem(item.name);
                }
                if (onItemCheckPasses != null)
                {
                    onItemCheckPasses.Invoke();
                }
            } else {
                if (onItemCheckFails != null)
                {
                    onItemCheckFails.Invoke();
                }
            }
        } else {
            ResetRunFlag();
        }
    }

}


// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.Events;
// using Yarn.Unity;

// public class OnItemUnityEventTrigger : UseInventoryItemBase 
// {
//     public UnityEvent onItemCheckPasses;
//     public UnityEvent onItemCheckFails;

//     override protected void OnActivate()
//     {
//         if (item == null) {
//             Debug.LogError("The UseInventoryItem(OnTarget) has no 'item' property set. The behaviour will not activate.");
//         }
//         if (inventory != null && item != null && 
//                 (inventory.HasItem(item.name) != invertItemCheck)) {
//             if (shouldRemoveFromInventory) {
//                 inventory.RemoveItem(item.name);
//             }
//             SetPlayerActivatableState(true);
//         } else {
//             ResetRunFlag();
//         }
//     }    

//     // override protected void OnActivate()
//     // {        
//     //     if (targetEvent != null)
//     //     {
//     //         targetEvent.Invoke();
//     //     }
//     // }

// }

// // using System.Collections;
// // using System.Collections.Generic;
// // using UnityEngine;

// // public class OnItemUnityEventTrigger : MonoBehaviour
// // {
// //     // Start is called before the first frame update
// //     void Start()
// //     {
        
// //     }

// //     // Update is called once per frame
// //     void Update()
// //     {
        
// //     }
// // }
