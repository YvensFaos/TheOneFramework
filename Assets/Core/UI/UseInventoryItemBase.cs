using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseInventoryItemBase : PlayerActivatable
{
    public Inventory inventory;
    [SerializeField] InventoryItemDefinition item;
    [SerializeField] bool shouldRemoveFromInventory = true;

    private GameObject target = null;

    [SerializeField] private bool invertItemCheck = false;

    public void SetTarget(GameObject target) {
        this.target = target;
        SetPlayerActivatableState(false);
    }

    void SetPlayerActivatableState(bool state) {
        if (target != null) {
            foreach (PlayerActivatable playerActivatable in target.GetComponents<PlayerActivatable>()) {
                if (playerActivatable != this) {
                    if (playerActivatable.canBeActivated != state) {
                        playerActivatable.canBeActivated = state;
                        if (state) {
                            playerActivatable.Activate();
                        }
                    }
                }
            }
        }
    }

    [System.Obsolete]
    void Start() {
        if (inventory == null) {
            inventory = FindFirstObjectByType<Inventory>();
        }
        SetPlayerActivatableState(false);
    }

    override protected void OnActivate()
    {
        if (item == null) {
            Debug.LogError("The UseInventoryItem(OnTarget) has no 'item' property set. The behaviour will not activate.");
        }
        if (inventory != null && item != null && 
                (inventory.HasItem(item.name) != invertItemCheck)) {
            if (shouldRemoveFromInventory) {
                inventory.RemoveItem(item.name);
            }
            SetPlayerActivatableState(true);
        } else {
            ResetRunFlag();
        }
    }

}

