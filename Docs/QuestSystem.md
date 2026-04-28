# 🛠 System: Quest System

## 📝 Overview

The **Quest System** is a modular, event-driven framework designed to handle the full lifecycle of a player's journey—from discovering and accepting quests to progression tracking and persistent JSON serialization.

The system utilizes a strict separation of concerns, splitting the architecture into three distinct layers: **Static Data** (what the quest is), **Runtime State** (the live progress of the quest), and **UI Presentation** (how the quest looks to the player). By leveraging a central `QuestManager` and an `EventBus`, the system allows designers to build multi-objective quests in the Unity Inspector without writing new code for every unique quest.

---

## 🛑 The Challenge (The "Problem")

Standard quest implementations often result in monolithic scripts where the quest logic is tightly coupled to every enemy, item, or NPC in the game.

- **Complexity:** Without an event-driven approach, every enemy death or item pickup requires a direct reference to the `QuestManager` to check for progress, leading to a "spaghetti" mess of dependencies.
- **Data Mutation:** Modifying `ScriptableObject` values directly at runtime (e.g., `questData.currentKills++`) is a common error that leads to permanent data corruption in the Unity Editor and inconsistent behavior in builds.
- **UI Entanglement:** If quest logic and UI updates are handled in the same script, the system becomes rigid. Adding a new UI element or changing the menu layout would require refactoring the core backend logic.
- **Scalability:** Hardcoding quest logic forces developers to create new C# scripts for every unique quest type, making the game increasingly difficult to maintain.

---

## 🏗 The Architecture (The "Solution")

The system is built on a "Layered Data" approach, ensuring that static asset data is never modified and communication between systems remains anonymous.

* **`QuestSo` & `QuestObjective`**: These are the **Static Data** assets. They act as immutable blueprints that define the IDs, names, descriptions, and required target amounts for objectives.
* **`QuestActive`**: This is the **Runtime Layer**. It is a serializable wrapper class that holds a reference to a `QuestSo` but tracks its own `ObjectiveProgress` array. This keeps the original asset clean while allowing for unique progress tracking per save file.
* **`QuestManager`**: The **Logic Layer**. It maintains a global quest database, manages the active quest list, handles save/load operations, and listens to the `EventBus` to process global game events into specific objective progress.
* **`QuestUI` & `Quest.cs`**: The **Presentation Layer**. These scripts are strictly responsible for the visual state, mapping the backend `QuestActive` data to `TextMeshPro` elements in the UI panel.

### 🧩 Patterns & Principles Used:

* **Observer Pattern (EventBus):** Decouples the world from the quests. Enemies and items fire generic "Update" events; the `QuestManager` observes these and decides which active quests are affected.
* **Wrapper Pattern:** `QuestActive` wraps the `QuestSo` to provide live state tracking without mutating the underlying ScriptableObject asset.
* **Data-Driven Design:** Quests are composed in the Inspector by dragging `QuestObjective` assets into a `QuestSo`, allowing for non-technical quest creation.
* **Interface Segregation (`ISaveable`):** The `QuestManager` implements a shared save interface, ensuring it plugs into the persistence pipeline without the `SaveManager` needing specific knowledge of quest logic.

---

## 💻 Code Highlight

The `QuestActive` class is the structural heart of the system. It uses **Constructor Overloading** to provide a clean way to either initialize a brand-new quest or rebuild an existing quest from a save file.

```csharp
[System.Serializable]
public class QuestActive
{
    public QuestSo QuestData { get; private set; }
    public int[] ObjectiveProgress { get; private set; } 
    public bool IsCompleted { get; private set; }

    // Constructor 1: Setup a BRAND NEW quest from a blueprint
    public QuestActive(QuestSo questData)
    {
        QuestData = questData;
        // Dynamically initialize the progress tracking array to match the SO
        ObjectiveProgress = new int[QuestData.QuestObjectives.Count];
        IsCompleted = false;
    }
    
    // Constructor 2: Rebuild a quest from Saved Data
    public QuestActive(QuestSo questData, int[] objectiveProgress, bool isCompleted)
    {
        QuestData = questData;
        ObjectiveProgress = new int[QuestData.QuestObjectives.Count];

        if (objectiveProgress != null)
        {
            // Safety Check: Ensure we don't go out of bounds if the SO was 
            // changed after the save was created.
            int count = Mathf.Min(objectiveProgress.Length, ObjectiveProgress.Length);
            for (int i = 0; i < count; i++)
            {
                ObjectiveProgress[i] = objectiveProgress[i];
            }
        }
        IsCompleted = isCompleted;
    }
}
```

The `HandleObjectiveUpdate` method in the `QuestManager` showcases the Loose Coupling of the system. It uses string-based `TargetIDs` to resolve progress. Whether the player killed a "Slime" or discovered a "Hidden Cave," the logic remains identical and agnostic to the object type.

```csharp
private void HandleObjectiveUpdate(string targetID, int amount)
{
    foreach (QuestActive questActive in _questList)
    {
        if (questActive.IsCompleted) continue; // Performance: Skip quests already done
        
        // Check every objective in the quest against the incoming TargetID
        for (int i = 0; i < questActive.QuestData.QuestObjectives.Count; i++)
        {
            if (questActive.QuestData.QuestObjectives[i].TargetID == targetID)
            {
                // Logic Layer: Update data
                questActive.AddObjectiveProgress(i, amount);
                
                // Presentation Layer: Notify UI to refresh
                questUI.UpdateQuestUI(questActive);
                Debug.Log($"Quest {questActive.QuestData.QuestName} updated.");
            }
        }
    }
}
```
