# 🛠 System: Interaction System

## 📝 Overview

The **Interaction System** provides a decoupled way for the player to interact with world objects such as NPCs, chests, and future interactable objects.

Currently, the main interactions are triggering NPC dialogue and opening chests, but the system is designed to expand without rewriting the player logic. Any object can become interactable by implementing the `IInteractable` interface, allowing the player to interact with it through a shared contract instead of checking for specific object types.

The system also supports priority-based targeting by tracking nearby interactables and automatically selecting the closest valid one.

---

## 🛑 The Challenge (The "Problem")

A common approach would be to put all interaction logic directly inside the player controller, checking object types manually whenever the player presses the interact button.

For example, the player might check: “Is this an NPC? Is this a chest? Is this an item? Is this a quest giver?” This works for a few objects, but quickly becomes hard to maintain.

- **Complexity:** The player script would become filled with long `if` / `else` or `switch` chains for every interactable object type.
- **Dependencies:** The player should not need to know how NPC dialogue, chest inventories, quest objects, or future systems work internally.
- **Scalability:** Adding 100 new interactable types should not require editing the player interaction code 100 times. Each object should own its own interaction behavior.

---

## 🏗 The Architecture (The "Solution")

The system is built around a simple interface contract: `IInteractable`.

Any object that wants to be interacted with implements:
```csharp
public interface IInteractable
{
  public bool CanInteract();
  public void Interact(PlayerController playerController);
}
```

The player interaction detector does not care what the object is. It only cares whether the object can interact and what method to call when the player presses the interaction input.

When objects enter the player’s interaction trigger, they are added to a list of nearby interactables. Each frame, the system removes invalid targets, selects the closest valid interactable, and displays the interact icon.

### 🧩 Patterns & Principles Used:

* **Interface-Driven Design:** `IInteractable` allows NPCs, chests, and future objects to share the same interaction contract without inheriting from a specific base class.
* **Single Responsibility Principle:** The player detector finds and selects interactables, while each interactable object owns its own behavior.
* **Open/Closed Principle:** New interactable types can be added by implementing `IInteractable` without modifying the player interaction detector.
* **Priority-Based Targeting:** When multiple interactables are nearby, the system chooses the closest valid target.
* **Defensive Validation:** `CanInteract()` allows objects to reject interaction if they are unavailable, already opened, disabled, or in an invalid state.

---

## 💻 Code Highlight
This method is the core of the targeting logic. It keeps the current interaction target accurate by first removing invalid objects, then selecting the closest valid interactable.

The LINQ ordering also makes the priority rule clear and readable: when multiple interactables are in range, the closest one becomes the active target. This creates predictable player behavior while keeping the system flexible for future expansion.
```csharp
private void UpdateTarget()
{
  // 1. Clean the list of objects that are no longer interactable
  _interactablesInRange.RemoveAll(i => i == null || !i.CanInteract());
  if (_interactablesInRange.Count == 0)
  {
      _interactableTarget = null;
      interactIcon.SetActive(false);
      return;
  }
  
  // 2. Set the target to the closest target
  _interactableTarget = _interactablesInRange
      .OrderBy(i => Vector2.Distance(transform.position, ((MonoBehaviour)i).transform.position))
      .FirstOrDefault();
  
  interactIcon.SetActive(true)
}
```
