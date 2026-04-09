using UnityEngine;

public class MouseAttackModule : AttackModule
{

    public override void Execute(CombatContext combatContext)
    {
        PrepareHitbox(combatContext.mousePosition);
        ApplyCombatLogic(combatContext);
        TriggerVisuals(combatContext.mousePosition);
    }

    private void PrepareHitbox(Vector2 position)
    {
        hitbox.transform.position = position;

        if (hitbox is HitBoxCircle hitboxCircle) hitboxCircle.radius = attackSize;
        // add more shape logic here if needed
    }

    private void ApplyCombatLogic(CombatContext combatContext)
    {
        // Pass the source 
        if (baseDamageData.source == null) baseDamageData.source = combatContext.source;
        
        hitbox.CheckForHits(baseDamageData);
    }

    private void TriggerVisuals(Vector2 position)
    {
        if (attackFX == null) return;
        
        GameObject fxInstance = Instantiate(attackFX, position, Quaternion.identity);
        
        hitbox.ScaleVisual(fxInstance);
    }
}
