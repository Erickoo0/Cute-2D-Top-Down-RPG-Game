using UnityEngine;

public abstract class HitBox : MonoBehaviour
{
    [Header("Base Settings")] 
    public LayerMask victimLayer; // Layer to check for collisions
    
    // Every hitbox must implement this and define their own logic to check for hits
    public abstract void CheckForHits(DamageData data);

    protected void SendDamage(DamageData data, Collider2D victimCollider)
    {
        if (victimCollider.TryGetComponent<IDamagable>(out IDamagable victim))
        {
            victim.TakeDamage(data);
        }
    }
}
