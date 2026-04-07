using UnityEngine;

[RequireComponent(typeof(Collider2D)), RequireComponent(typeof(Rigidbody2D)), RequireComponent(typeof(Health))]
public class HurtBox : MonoBehaviour, IDamagable
{
    private Health _health;
    //private Animator _animator;

    private void Awake()
    {
        _health = GetComponent<Health>();
        //_animator = GetComponent<Animator>();
    }

    public void TakeDamage(DamageData data)
    {
        _health.HpCurrent -= data.damageAmount;
        
        //_animator.SetTrigger("Hurt");
        
        ApplyKnockback(data.hitDirection, data.knockbackForce);
    }

    public void ApplyKnockback(Vector2 direction, float knockbackForce)
    {
        // Knockback logic later
    }
}
