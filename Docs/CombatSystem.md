# 🛠 System: Combat System

## 📝 Overview

The **Combat System** defines how attacks are configured, triggered, detected, and applied to damageable entities.

The system is built around reusable attack data, hitboxes, damage payloads, and damage receivers. `AttackData` stores configurable attack settings, `DamageData` packages the result of an attack, `HitBox` handles collision detection, and `IDamagable` defines the contract for anything that can receive damage.

Currently, the system supports mouse-based attacks, charge attack data, knockback, health damage, hit feedback, camera impulse, and visual flash effects. The design allows future weapons, abilities, enemy attacks, and elemental damage types to reuse the same combat pipeline.

---

## 🛑 The Challenge (The "Problem")

A common beginner approach is to put all combat behavior directly into the player or enemy script: check input, detect collisions, subtract health, play effects, apply knockback, and trigger animations all in one place.

- **Complexity:** Combat becomes hard to maintain if attack input, hit detection, damage logic, knockback, and feedback effects are all mixed together.
- **Dependencies:** The attacker should not need to know the exact class of the target. A player attack, enemy attack, trap, or spell should all be able to damage anything that follows the same damage contract.
- **Scalability:** Adding new attacks, weapons, elemental types, or enemy abilities should not require rewriting the health system or duplicating collision logic.

---

## 🏗 The Architecture (The "Solution")

The combat flow is split into independent pieces:

* **`AttackData`**: A ScriptableObject defining reusable attack settings such as hitbox prefab, damage data, radius, offset, and hit behavior. Most attacks will have a unique AttackData SO.
* **`DamageData`**: A lightweight struct that packages damage amount, direction, knockback, damage type, and source object. Each AttackData has a DamageData struct inside.
* **`HitBox`**: An abstract base class for hit detection. Specific hitbox types can define their own collision logic while reusing shared damage-sending behavior.
* **`IDamagable`**: An interface implemented by anything that can receive damage.
* **`HurtBox`**: A damage receiver that applies health loss, knockback, camera impulse, and hit flash feedback.

### 🧩 Patterns & Principles Used:

* **Interface-Driven Design:** `IDamagable` allows attacks to damage any valid target without knowing its concrete class.
* **ScriptableObject Data Architecture:** `AttackData` makes attacks designer-configurable and reusable across players, enemies, and abilities.
* **Template Method Style:** `HitBox` defines shared damage-sending logic while letting subclasses implement their own `CheckForHits()` behavior.
* **Separation of Concerns:** Attack configuration, hit detection, damage data, and damage response are split into separate classes.
* **Open/Closed Principle:** New attack types or hitbox shapes can be added by extending existing abstractions instead of modifying the entire combat system.

---

## 💻 Code Highlight

This method is a stronger representation of the Combat System because it shows the full attack execution pipeline. The attack state does not hardcode damage, hitbox size, or visual effects. Instead, it reads from `MouseAttackData`, spawns the configured hitbox prefab, applies the configured radius, optionally scales the visual effect, injects the attacker as the damage source, and then processes the hit.

A strong design choice here is the use of **data-driven combat execution**:

```csharp
 public override void Enter()
{
   _isFinished = false;
  
  // Safety Check
  if (_attackData == null)
  {
      _isFinished = true;
      return;
  }
  
  // 1. Spawn Hitbox exactly at the mouse position
  HitBox spawnedHitbox = Object.Instantiate(
      _attackData.hitboxPrefab,
      _combatController.CombatContext.mousePosition,
      Quaternion.identity
  );
  spawnedHitbox.enableHitbox = true;
  
  // 2. Configure Hitbox Radius
  if (spawnedHitbox is HitBoxCircle circle) 
  { 
      circle.radius = _attackData.hitboxRadius; 
  }
  
  // 3. Trigger Visuals
  if (_attackData.attackFX != null)
  {
      GameObject fxInstance = Object.Instantiate(
          _attackData.attackFX,
          _combatController.CombatContext.mousePosition,
          Quaternion.identity
      );
  
      spawnedHitbox.ScaleVisual(fxInstance);
  }
  
  // 4. Process Damage using the injected context
  DamageData executionDamage = _attackData.damageData;
  executionDamage.source = _combatController.CombatContext.source;
  spawnedHitbox.CheckForHits(executionDamage);
  
  // 5. Clean up hierarchy immediately for an instant attack
  Object.Destroy(spawnedHitbox.gameObject);
  _isFinished = true;
}
```

This method demonstrates the hit detection side of the Combat System. It performs an area check using `Physics2D.OverlapCircleAll`, filters out the attacker to prevent self-hits, calculates a knockback direction per target, copies the original `DamageData`, modifies the direction for that specific victim, and then sends the final damage payload.

```csharp
bool hitSucess = false;
Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, radius, victimLayer);

foreach (Collider2D hit in hits)
{
    // Skip self
    if (hit.gameObject == data.source) continue;

    // 1. Calculate a direction for knockback
    Vector2 targetPosition = hit.transform.position;
    Vector2 attackPosition = transform.position;
    Vector2 primaryKnockbackDirection = (targetPosition - attackPosition).normalized;
    
    // 2. Calculate perpendicular offset
    Vector2 perpendicularDirection = new Vector2(
        -primaryKnockbackDirection.y,
        primaryKnockbackDirection.x
    ); 
    
    // 3. Determine the side
    float sideSign =
        (primaryKnockbackDirection.x * perpendicularDirection.y -
         primaryKnockbackDirection.y * perpendicularDirection.x) > 0 ? 1f : -1f;   

    // 4. Pass the direction
    Vector2 finalDirection =
        (primaryKnockbackDirection +
         (perpendicularDirection * (sideSign * data.knockbackForce))).normalized;
    
    // 5. Create a copy of passed-in damage data and direction
    DamageData finalData = data;
    finalData.hitDirection = finalDirection;

    // 6. Send the damage data
    if (SendDamage(finalData, hit))
    {
        hitSucess = true;
    }
}

return hitSucess;
```
