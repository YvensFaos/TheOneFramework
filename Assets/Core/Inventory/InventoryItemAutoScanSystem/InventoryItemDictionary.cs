using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/InventoryItemDictionary", order = 1)]
public class InventoryItemDictionary : ScriptableObject
{
    public List<InventoryItemDefinition> items = new List<InventoryItemDefinition>();

}