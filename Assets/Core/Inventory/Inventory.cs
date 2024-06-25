using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class Inventory : MonoBehaviour
{
    public int numberOfSlots = 5;
    private List<InventoryItemDefinition> items = new List<InventoryItemDefinition>();

    public delegate void InventoryChanged();
    public event InventoryChanged OnInventoryChanged;

    void Start()
    {
        while (items.Count < numberOfSlots)
        {
            items.Add(null);
        }
    }

    public int GetItemCount() {
        return items.Count;
    }

    public InventoryItemDefinition GetItem(int index) {
        return items[index];
    }

    public void AddInventoryItem(InventoryItemDefinition item)
    {
        RemoveInventoryItem(item.name);
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i] == null)
            {
                items[i] = item;
                OnInventoryChanged?.Invoke();
                break;
            }
        }
    }

    public void RemoveInventoryItem(string id)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i] != null && items[i].name == id)
            {
                items[i] = null;
                OnInventoryChanged?.Invoke();
                break;
            }
        }
    }

    // public bool HasItem(InventoryItemDefinition itemToCheck) {
    //     foreach (var item in items) {
    //         if (itemToCheck == item) return true;
    //     }
    //     return false;
    // }

    public bool HasItem(string id)
    {
        foreach (var item in items)
        {
            if (item != null && item.name == id)
            {
                return true;
            }
        }
        return false;
    }

    public void AddItem(string id)
    {
        InventoryItemDefinition item = InventoryItemDefinitions.GetInventoryItemByName(id);
        if (item != null)
        {
            AddInventoryItem(item);
        }
    }

    public void RemoveItem(string id)
    {
        RemoveInventoryItem(id);
    }

    [YarnFunction("has_item")]
    public static bool HasItemStatic(string targetName, string id)
    {
        GameObject target = GameObject.Find(targetName);
        if (target != null) {
            Inventory inventory = target.GetComponent<Inventory>();
            if (inventory) {
                return inventory.HasItem(id);
            }
        }
        return false;
    }
}
