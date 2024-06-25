using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace StarterAssets {

    public class RightMouseButtonLook : MonoBehaviour
    {
        private bool currentMouseState = true;
        void Update() {
            var newMouseState = Mouse.current.rightButton.isPressed;
            if (newMouseState != currentMouseState) {
                if (newMouseState) {
                    Cursor.lockState = CursorLockMode.Locked;
                } else {
                    Cursor.lockState = CursorLockMode.None;
                }
                currentMouseState = newMouseState;
            }  
        }

    }

}