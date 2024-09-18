using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
 
public class LoadScene : PlayerActivatable
{
    public string sceneToLoad;
    public List<GameObject> ObjectsToKeep;  // Public list for objects to keep
    private bool isReady = true;
    private static List<GameObject> keptObjects = new List<GameObject>(); // Track kept objects
 
    override protected void OnActivate()
    {
        isReady = false;
 
        // Loop through each object in the list and apply DontDestroyOnLoad
        foreach (GameObject obj in ObjectsToKeep)
        {
            if (obj != null && !keptObjects.Contains(obj))
            {
                DontDestroyOnLoad(obj);
                keptObjects.Add(obj);  // Track the object
            }
        }
 
        if (!string.IsNullOrEmpty(sceneToLoad))
        {
            SceneManager.sceneLoaded += OnSceneLoaded;  // Subscribe to scene loaded event
            SceneManager.LoadScene(sceneToLoad);
        }
    }
 
    override protected bool IsActivated()
    {
        return !isReady;
    }
 
    // This is called when the scene has finished loading
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;  // Unsubscribe from event
 
        // Move each kept object to the current scene
        foreach (GameObject obj in keptObjects)
        {
            if (obj != null)
            {
                SceneManager.MoveGameObjectToScene(obj, scene);
            }
        }
    }
}
 