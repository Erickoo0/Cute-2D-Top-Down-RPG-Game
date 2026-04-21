using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EntityMover : MonoBehaviour
{
    [Header("Movement Settings")] 
    public float moveSpeed = 5f;
    
    [Header("Knockback Settings")]
    [SerializeField] private float knockbackDecay = 8f;
    private bool _isKnockedBack = false;
    private float _knockbackTimer;
    
    private Rigidbody2D _rigidbody;
    private Vector2 _moveDirection;
    private Collider2D _collider;
    
    public Vector2 MoveDirection => _moveDirection;
    public bool IsKnockedBack => _isKnockedBack;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();
    }

    private void FixedUpdate()
    {
        if (PauseManager.IsGamePaused)
        {
            _rigidbody.linearVelocity = Vector2.zero;
            return;
        }

        if (_isKnockedBack)
        {
            HandleKnockbackLoop();
        }
        else
        {
            HandleNormalMovement();
        }
    }

    public void SetMoveDirection(Vector2 direction)
    {
        _moveDirection = direction.normalized;
    }

    private void HandleKnockbackLoop()
    {
        // Knockback decay formula
        _rigidbody.linearVelocity = Vector2.MoveTowards(_rigidbody.linearVelocity, Vector2.zero, knockbackDecay * Time.fixedDeltaTime * 10f);
        
        _knockbackTimer -= Time.fixedDeltaTime;
        
        // Snap to finish
        if (_knockbackTimer <= 0f || _rigidbody.linearVelocity.sqrMagnitude < 0.1f)
        {
            _isKnockedBack = false;
            _rigidbody.linearVelocity = Vector2.zero;
        }
    }

    private void HandleNormalMovement()
    {
        _rigidbody.linearVelocity = _moveDirection * moveSpeed;
    }
    

    public void ApplyKnockback(Vector2 direction, float force, float duration, GameObject source = null)
    {
        if (!gameObject.activeInHierarchy) return;

        _isKnockedBack = true;
        _knockbackTimer = duration;
        
        // Immediate velocity burst
        _rigidbody.linearVelocity = direction * force;

        // Temporarily ignore collision with the attacker
        if (source != null)
        {
            StartCoroutine(TemporaryIgnoreCollision(source, duration * 1.5f));
        }
    }
    
    private IEnumerator TemporaryIgnoreCollision(GameObject source, float duration)
    {
        if (source.TryGetComponent<Collider2D>(out Collider2D sourceCol) && _collider != null)
        {
            Physics2D.IgnoreCollision(_collider, sourceCol, true);
            yield return new WaitForSeconds(duration);
            Physics2D.IgnoreCollision(_collider, sourceCol, false);
        }
    }
}
