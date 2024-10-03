using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;
using StarterAssets;

public class TogglePlayerControls : MonoBehaviour
{
    public void SetInputState(bool enabled)
    {
        GetComponent<CharacterController>().enabled = enabled;
        GetComponent<ThirdPersonControllerFixed>().enabled = enabled;
        GetComponent<Animator>().enabled = enabled;
    }
    
    public void EnableControls() {
        SetInputState(true);
    }

    public void DisableControls() {
        SetInputState(false);
    }
}