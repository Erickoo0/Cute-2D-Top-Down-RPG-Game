# 🛠 System: State Machine System

## 📝 Overview
The **State Machine System** is a highly modular, generic Finite State Machine (FSM) architecture that drives the behavior of all actors in the framework, from the player character to complex AI mobs. 

By utilizing a central `StateMachine` component alongside a generic `State<T>` abstract class, the system allows discrete behaviors (like Idle, Wander, Chase, or Dash) to be cleanly separated into their own classes. Furthermore, thanks to custom property attributes (`[SubclassSelector]`), these states are exposed polymorphically in the Unity Inspector, allowing designers to plug-and-play different behaviors without altering the core controller logic.

---

## 🛑 The Challenge (The "Problem")
The most common beginner approach to entity behavior is using massive `switch(state)` statements or endless `if/else` blocks inside a single `Update()` loop (e.g., `if (isChasing) { ... } else if (isWandering) { ... }`). 

- **Complexity:** As an entity's logic grows to include attacking, fleeing, getting stunned, or dying, the core script becomes an unmaintainable "God Class" filled with spaghetti code.
- **Dependencies:** If states are hardcoded into the enemy or player scripts, they become tightly coupled. A `PlayerController` shouldn't need to know the inner mathematical workings of a "Dash" state; it just needs to know that it *is* dashing.
- **Scalability:** If behavior logic is baked into a specific `GoblinController`, you cannot easily reuse that exact same "Wander" logic for a `SlimeController` or an NPC. You end up copying and pasting code across multiple scripts.

---

## 🏗 The Architecture (The "Solution")
This system solves the problem by treating states as objects rather than mere enumerations or boolean flags. It leverages a strong generic foundation to ensure type safety while maintaining maximum flexibility.

### 🧩 Patterns & Principles Used:
* **State Pattern:** Encapsulates specific behaviors into discrete objects (`BaseIdleState`, `BaseChaseState`, etc.). The central `StateMachine` simply asks the current state to `Update()`, delegating the actual work to the state object itself.
* **Generic Typing (`State<T>`):** By passing the specific controller type into the state (e.g., `State<PlayerController>`), the state can directly access required properties (like `MovementInput` or `TargetableList`) without expensive `GetComponent` calls or unsafe casting.
* **Open/Closed Principle:** To add a new behavior (like a "Flee" state), you simply create a new script inheriting from `BaseActionState`. The `EntityController` and `StateMachine` classes do not need to be modified at all.
* **Inspector-Driven Polymorphism:** Utilizing `[SerializeReference]` and a custom `[SubclassSelector]` attribute allows the system to serialize abstract/inherited classes directly in the Unity Inspector. This makes the architecture extremely designer-centric.

---

## 💻 Code Highlights

### Strongly-Typed Generic States
This abstract class is the backbone of the modularity. Instead of a generic state that forces you to constantly cast `(PlayerController)controller` every frame, the generic `State<T>` ensures that when an `IdleState` is created for a Player, it natively understands it is dealing with a `PlayerController`. This enforces strict type safety and cleaner autocomplete when writing state logic.

```csharp
[System.Serializable]
public abstract class State<T> : State where T : BaseEntityController
{
    // Specialized Reference holds the exact controller (PlayerController or EntityController)
    protected T controller;

    public virtual void Setup(T controller, StateMachine stateMachine)
    {
        this.controller = controller;
        this.stateMachine = stateMachine;
    }
}
```

### Designer-Friendly Polymorphic Serialization
Instead of hardcoding which `IdleState` or `WanderState` an enemy uses, the `EntityController` exposes abstract references to the inspector. 
Combined with the custom `[SubclassSelector]` attribute, a designer can click a dropdown in the Unity Inspector

```csharp
public class EntityController : BaseEntityController
{
    // ... targeting and mob data ...

    [Header("State References")]
    // [SerializeReference] tells Unity to serialize the object by reference, allowing polymorphism
    // [SubclassSelector] triggers the custom editor GUI to pick derived classes
    [SerializeReference, SubclassSelector] public BaseIdleState IdleState;
    [SerializeReference, SubclassSelector] public BaseWanderState WanderState;
    [SerializeReference, SubclassSelector] public BaseChaseState ChaseState;
    [SerializeReference, SubclassSelector] public BaseActionState AttackState;
    
    protected override void Awake()
    {
        base.Awake();
        
        // Inject dependencies into the inspector-assigned states
        IdleState?.Setup(this, StateMachine);
        WanderState?.Setup(this, StateMachine);
        ChaseState?.Setup(this, StateMachine);
        AttackState?.Setup(this, StateMachine);
        
        SetupWaypointsList();
    }
}
```
