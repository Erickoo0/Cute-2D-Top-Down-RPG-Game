using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class MenuController : MonoBehaviour
{
    [SerializeField] private GameObject menuPanel;
    
    public void ToggleMenu(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        if (!menuPanel.activeSelf)EventBus.RequestOpenMenu(menuPanel);
        else if (menuPanel.activeSelf) EventBus.RequestCloseMenu(menuPanel);
    }
}
