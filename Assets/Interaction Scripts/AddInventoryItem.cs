using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddInventoryItem : PlayerActivatable
{
    public Inventory inventory;
    public InventoryItemDefinition item;


    override protected void OnActivate()
    {
        if (inventory != null) {
            inventory.AddItem(item.name);
        }
    }
}
