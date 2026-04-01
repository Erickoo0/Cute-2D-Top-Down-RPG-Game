using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerStatsManager : MonoBehaviour
{
    [SerializeField] private GameObject playerStatsPanel;
    
    public void ToggleMenu(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        if (!playerStatsPanel.activeSelf)EventBus.RequestOpenMenu(playerStatsPanel);
        else if (playerStatsPanel.activeSelf) EventBus.RequestCloseMenu(playerStatsPanel);
    }
}
