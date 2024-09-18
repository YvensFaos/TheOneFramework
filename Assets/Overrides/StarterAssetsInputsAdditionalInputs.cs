using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace StarterAssets
{
	public class StarterAssetsInputsAdditionalInputs : StarterAssetsInputs
    {
        public bool interact;

		public bool inventory;

		public UnityEvent OnInventoryButton;


        public void OnInteract(InputValue value) 
		{
			InteractInput(value.isPressed);
		}

		public void InteractInput(bool newInteractState) 
		{
			interact = newInteractState;
		}

		public void OnInventory(InputValue value) 
		{
			InventoryInput(value.isPressed);
		}

		public void InventoryInput(bool newInventoryState) {
			inventory = newInventoryState;
			if (newInventoryState) {
				if (OnInventoryButton != null) {
					OnInventoryButton.Invoke();
				}
			}
		}

		bool isLookingAround = false;

		public void OnLookAround(InputValue value)
		{
			if (!isLookingAround) {
				isLookingAround = true;
				return;
			}
			if(cursorInputForLook)
			{
				var lookAroundValue = value.Get<Vector2>();
				//if (lookAroundValue.magnitude < 5.0f) { //input system ftw
					LookInput(lookAroundValue);
				//}
			}
		}

		void Update() {
			if (!Mouse.current.rightButton.isPressed) {
				isLookingAround = false;
			}
		}
    }
}
