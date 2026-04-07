using UnityEngine;

public class HitBoxCircle : HitBox
{
    public float radius = 1f;

    public override void CheckForHits(DamageData data)
    {
        // Check collisions in a circle and asign to an array
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, radius, victimLayer);
        
        // For every collision in the array, damage them
        foreach (Collider2D hit in hits) SendDamage(data, hit);
    }
    
    private void OnDrawGizmos() { Gizmos.color = Color.red; Gizmos.DrawWireSphere(transform.position, radius); }
}
