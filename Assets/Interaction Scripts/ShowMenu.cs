using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowMenu : PlayerActivatable
{
    public GameObject menuToActivate;
    private bool isShowing = false;

    override protected void OnActivate()
    {
        if (menuToActivate)
        {
            isShowing = true;
            menuToActivate.SetActive(true);
            StartCoroutine(WaitForMenuDisable());
        }
    }

    private IEnumerator WaitForMenuDisable()
    {
        while (menuToActivate.activeSelf)
        {
            yield return null;
        }
        isShowing = false;
    }

    override protected bool IsActivated() 
    {
        return isShowing;
    }

}
