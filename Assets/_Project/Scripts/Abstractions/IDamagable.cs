using UnityEngine;

public interface IDamagable
{
    void TakeDamage(DamageData data);
    void ApplyKnockback(Vector2 direction, float knockback);
}
