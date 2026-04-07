using UnityEngine;

public class MouseAttackModule : AttackModule
{
    public override void Execute(CombatContext combatContext)
    {
        hitbox.transform.position = combatContext.mousePosition;

        if (baseDamageData.source == null) baseDamageData.source = combatContext.attacker;
        
        hitbox.CheckForHits(baseDamageData);
    }
}
