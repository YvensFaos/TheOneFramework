using System;
using UnityEngine;
using System.Collections;

public class InventoryItemDefinitions
{
    private static InventoryItemDictionary inventoryItemDictionary;

    public static InventoryItemDefinition GetInventoryItemByName(string name) {

        if (inventoryItemDictionary == null) {
            inventoryItemDictionary = Resources.Load<InventoryItemDictionary>("InventoryItemDictionary");
        }

        if (inventoryItemDictionary != null)
        {
            foreach (InventoryItemDefinition item in inventoryItemDictionary.items)
            {
                if (item.name == name) {
                    return item;
                }
            }
            Debug.LogError(string.Format("InventoryItem '{0}' cannot be found", name));
        }
        else
        {
            Debug.LogError("InventoryItemDictionary asset could not be found in Resources.");            
        }

        return null;
    }
}