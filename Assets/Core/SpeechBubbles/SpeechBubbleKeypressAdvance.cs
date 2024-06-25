using System;
using UnityEngine;

#if USE_INPUTSYSTEM && ENABLE_INPUT_SYSTEM
    using UnityEngine.InputSystem;
#endif

public class SpeechBubbleKeypressAdvance : MonoBehaviour
{
    [HideInInspector]
    public Action OnUserPressedAdvance;
    public KeyCode advanceKeyCode1 = KeyCode.E; 
    public KeyCode advanceKeyCode2 = KeyCode.Return;

#if USE_INPUTSYSTEM && ENABLE_INPUT_SYSTEM
    private InputAction action;
#endif

    void OnEnable()
    {
#if USE_INPUTSYSTEM && ENABLE_INPUT_SYSTEM
        action = new InputAction("Progress", InputActionType.Button, "<Keyboard>/" +  + advanceKeyCode2.ToString() + ",<Keyboard>/" + advanceKeyCode1.ToString());
        action.Enable();
        action.performed += OnActionTriggered;
#endif
    }

    void OnDisable()
    {
#if USE_INPUTSYSTEM && ENABLE_INPUT_SYSTEM
        if (action != null)
        {
            action.performed -= OnActionTriggered;
            action.Disable();
            action = null;
        }
#endif
    }

#if ENABLE_LEGACY_INPUT_MANAGER
    void Update()
    {
        if (Input.GetKeyDown(advanceKeyCode1) || Input.GetKeyDown(advanceKeyCode2))
        {
            AdvanceText();
        }
    }
#endif

#if USE_INPUTSYSTEM && ENABLE_INPUT_SYSTEM
    void OnActionTriggered(InputAction.CallbackContext context)
    {
        AdvanceText();
    }
#endif

    void AdvanceText()
    {
        if (OnUserPressedAdvance != null) {
            OnUserPressedAdvance();
        }
    }
}
