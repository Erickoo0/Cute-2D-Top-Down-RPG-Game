using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private Animator _animator;
    private PlayerMovement _playerMovement;

    void Start()
    {
        _animator = GetComponent<Animator>();
        _playerMovement = GetComponent<PlayerMovement>();
    }

    void FixedUpdate()
    {
        float speed = _playerMovement._moveInput.magnitude;
        if (speed > 0.1f)
        {
            _animator.SetBool("IsWalking", true);
            _animator.SetFloat("InputX", _playerMovement._moveInput.x);
            _animator.SetFloat("InputY", _playerMovement._moveInput.y);
            _animator.SetFloat("LastInputX", _playerMovement._moveInput.x);
            _animator.SetFloat("LastInputY", _playerMovement._moveInput.y);
        }
        else
        {
            _animator.SetBool("IsWalking", false);
        }
    }
}
