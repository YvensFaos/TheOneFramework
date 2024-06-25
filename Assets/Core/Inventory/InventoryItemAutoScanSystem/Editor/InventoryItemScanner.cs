using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;
using System.IO;

[InitializeOnLoad]
class InventoryItemScanner
{
    private static FileSystemWatcher watcher;

    static InventoryItemScanner()
    {
        string pathToWatch = Application.dataPath + "/InventoryItems";
        watcher = new FileSystemWatcher(pathToWatch);

        watcher.Changed += OnChanged;
        watcher.Created += OnChanged;
        watcher.Deleted += OnChanged;
        watcher.Renamed += OnRenamed;

        watcher.EnableRaisingEvents = true;

        // Optionally, set filters to monitor specific files or types.
        //watcher.Filter = "*.txt"; // Example: only monitor text files.
    }

    private static void OnChanged(object sender, FileSystemEventArgs e)
    {        
        EditorApplication.delayCall += () => {
            InventoryItemScanner.RefreshInventoryItems();
        };
    }

    private static void OnRenamed(object sender, RenamedEventArgs e)
    {
        EditorApplication.delayCall += () => {
            InventoryItemScanner.RefreshInventoryItems();
        };
    }

    static public void RefreshInventoryItems()
    {
        var inventoryItems = new List<InventoryItemDefinition>();

        string[] guids = AssetDatabase.FindAssets("t:InventoryItemDefinition", new[] { "Assets/InventoryItems" });
        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            InventoryItemDefinition item = AssetDatabase.LoadAssetAtPath<InventoryItemDefinition>(path);
            if (item != null)
            {
                inventoryItems.Add(item);
            }
        }

        InventoryItemDictionary inventoryItemDictionary = Resources.Load<InventoryItemDictionary>("InventoryItemDictionary");
        if (inventoryItemDictionary != null)
        {
            inventoryItemDictionary.items = inventoryItems;
            EditorUtility.SetDirty(inventoryItemDictionary);
            AssetDatabase.SaveAssets();
        }
        else
        {
            Debug.LogError("InventoryItemDictionary asset could not be found in Resources.");
        }
    }    
}
