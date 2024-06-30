using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : PlayerActivatable
{
    public string sceneToLoad;
    private bool isReady = true;

    override protected void OnActivate()
    {
        isReady = false;
        if (!string.IsNullOrEmpty(sceneToLoad))
        {
            SceneManager.LoadScene(sceneToLoad);
        }
    }

    override protected bool IsActivated()
    {
        return !isReady;
    }
}
