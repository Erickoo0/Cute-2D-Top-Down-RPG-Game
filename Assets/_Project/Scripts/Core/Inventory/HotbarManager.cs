using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class HotbarManager : MonoBehaviour
{
    // Reference to Hotbar Actions
    private PlayerInput _playerInput; 

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
    }

    /// <summary>
    ///  Moves the selection frame
    /// </summary>
    private void OnSelectSlot(InputAction.CallbackContext context)
    {
        // Detects key pressed down input
        if (context.ReadValueAsButton())
        {
            // Gets the name of the key pressed
            string keyName = context.control.name; 
            // Gets the last char of the keyName (e.g. keyboard1 -> 1)
            char lastChar = keyName[keyName.Length - 1];

            // Safety Check: Key is a number
            if (char.IsDigit(lastChar))
            {
                // Subtract 0 from any number gives you its actual integer value
                int val = lastChar - '0';
                // Converts 0 - > 10
                if (val == 0) val = 10; 
                // Converts Key pressed to Slot Index
                int targetIndex = val - 1;
                
                // Checks if targetIndex is in range of hotbar
                if (targetIndex >= 0 && targetIndex < 9)
                {
                    InventoryManager.Instance.ChangeActiveSlot(targetIndex);
                }
            }
        }
    }

    // Method to enable/disable hotbar input
    public void SetHotbarActive(bool active)
    {
        if (active) _playerInput.ActivateInput();
        else _playerInput.DeactivateInput();
    }
}