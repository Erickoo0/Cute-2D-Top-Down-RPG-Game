using UnityEngine;
using UnityEngine.InputSystem;

public class HotbarManager : MonoBehaviour
{
    [SerializeField] private InventoryUI inventoryUI;
    private PlayerInput _playerInput;

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
    }

    // This method handles the logic we discussed
    public void OnSelectSlot(InputAction.CallbackContext context)
    {
        if (context.ReadValueAsButton())
        {
            string keyName = context.control.name;
            char lastChar = keyName[keyName.Length - 1];

            if (char.IsDigit(lastChar))
            {
                int val = lastChar - '0';
                if (val == 0) val = 10; 

                int targetIndex = val - 1;
                
                if (targetIndex >= 0 && targetIndex < 9)
                {
                    inventoryUI.MoveSelectionFrame(targetIndex);
                    // NEW: Tell the InventoryManager which slot is now "Active"
                    Debug.Log($"Hotbar Slot {val} Selected");
                }
            }
        }
    }

    // Pro Tip: Methods to enable/disable hotbar input from other scripts
    public void SetHotbarActive(bool active)
    {
        if (active) _playerInput.ActivateInput();
        else _playerInput.DeactivateInput();
    }
}