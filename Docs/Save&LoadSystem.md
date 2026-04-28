# 🛠 System: Save & Load System

## 📝 Overview
The **Save & Load System** is a robust, JSON-based persistence framework designed to serialize and deserialize the game's state across sessions. 

Instead of relying on a massive, tightly coupled manager that knows about every single object in the game, this system utilizes an interface-driven approach (`ISaveable`). A central `SaveData` container is passed around the scene, allowing individual systems (like the Player, Inventory, and Quests) to pack and unpack their own data independently before the `SaveManager` writes it to the disk.

---

## 🛑 The Challenge (The "Problem")
A common trap when building save systems is creating a "God-like" `SaveManager` that holds hard references to the `PlayerController`, `InventoryManager`, `QuestLog`, and every chest in the game to manually extract their variables.

- **Complexity:** As the game grows, this God script becomes hundreds of lines long. If you delete a chest or rename a player variable, the entire save script breaks.
- **Dependencies:** The file I/O system (reading/writing to the hard drive) should absolutely not care about how many potions the player has. It should only care about writing text to a file. 
- **Scalability:** If a developer decides to add a brand new system halfway through development (e.g., a Farming system that tracks crop growth), a poorly designed manager requires rewriting the core save logic. 

---

## 🏗 The Architecture (The "Solution")
This system solves the dependency problem by inverting control. The `SaveManager` doesn't fetch data; it simply broadcasts an empty "box" (`SaveData`), asks everyone to put their stuff in it, and then seals the box.

### 🧩 Patterns & Principles Used:
* **Interface-Driven Design (`ISaveable`):** By making objects implement `ISaveable`, the manager can blindly gather them using polymorphism. It doesn't need to know *what* it's talking to, just that the object adheres to the save contract.
* **Data Transfer Object (DTO):** The `SaveData` class acts purely as a serializable data container. It holds no logic, ensuring that Unity's `JsonUtility` can cleanly convert it to and from text.
* **Open/Closed Principle:** To add a new saveable feature (like an NPC relationship system), you simply make the new NPC script implement `ISaveable` and add a variable to `SaveData`. The `SaveManager` script never needs to be touched again.
* **Singleton Pattern:** The `SaveManager` is implemented as a Singleton (`Instance`) to ensure there is only one authoritative script handling file locking and scene transitions at a time.



---

## 💻 Code Highlights

### Interface-Driven Data Collection
This is the core of the system's scalability. Instead of manually linking references in the inspector, the `SaveManager` dynamically finds every active `MonoBehaviour` in the scene that implements `ISaveable`. It then iterates through them, passing the central `saveData` container so each object can inject its own specific data. 

```csharp
public void SaveGame()
{
    // 1. Create a container to hold save data
    SaveData saveData = new SaveData();
    
    // 2. Find all ISaveable scripts in the scene dynamically
    var allSaveables = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None).OfType<ISaveable>();
    
    // 3. Export all save data from ISaveable into container
    foreach (ISaveable saveable in allSaveables) 
        saveable.PopulateSaveData(saveData);
    
    // 4. Convert and write
    string json = JsonUtility.ToJson(saveData);
    File.WriteAllText(_savePath, json);
}
```

### Safe Asynchronous Scene Loading
Loading save data into a scene that hasn't fully initialized leads to massive NullReference exceptions. By wrapping the scene transition in an `IEnumerator`, the system forces Unity to completely finish loading the new scene environment `(asyncLoad.isDone)` before it attempts to inject the JSON data into the new objects.

```csharp
private IEnumerator TransitionRoutine(string sceneName, bool isNewGame)
{
    // 1. Begin Loading Scene in the background
    AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
    
    // 2. Wait here until Unity completely finishes loading the scene
    if (asyncLoad != null)
        while (!asyncLoad.isDone) yield return null;
    
    // 3. Proceed with Load & Save Logic safely
    if (isNewGame) NewGame();
    else LoadGame();
}
```
