using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

[RequireComponent(typeof(PlayerController))]
public class PlayerCombatController : MonoBehaviour
{
    [Header("Attack Library")]
    [SerializeField] private List<AttackData> attackLibrary;
    
    private PlayerController _playerController;
    private Camera _mainCam;
    private Vector2 _rawMousePosition;

    private void Awake()
    {
        _mainCam = Camera.main;
        _playerController = GetComponent<PlayerController>();
    }
    
    
    //Tracks Mouse Movement
    public void OnPoint(InputAction.CallbackContext context)
    {
        _rawMousePosition = context.ReadValue<Vector2>();
    }
    
    public T GetAttackData<T>() where T : AttackData
    {
        foreach (var data in attackLibrary)
        {
            if (data is T specificData) return specificData;
        }
        return null;
    }

    public void OnMouseClick(InputAction.CallbackContext context)
    { 
        if (!context.performed) return;
        
        // Check the library for mouse attack data
        if (GetAttackData<MouseAttackData>() == null) return;
        
        // Gather the context 
        CombatContext contextData = new CombatContext
        {
            source = gameObject,
            mousePosition = _mainCam.ScreenToWorldPoint(_rawMousePosition),
            userPosition = transform.position,
            facingDirection = transform.right // Add logic later
        };
        
        // Execute attack module
        _playerController.MouseAttackState.SetupContext(contextData);
        _playerController.StateMachine.ChangeState(_playerController.MouseAttackState);
    }
}
