# 🛠 System: Inventory System

## 📝 Overview

The **Inventory System** manages how the player stores, stacks, removes, drops, saves, loads, and displays items.

The system is built around a clear separation between data and UI. `InventoryManager` owns the actual inventory data through an array of `ItemInstance` slots, while `InventoryUI` and `InventorySlotUI` handle the visual representation. This allows inventory logic to update independently from the interface, while UI elements simply listen for changes and refresh when needed.

The system currently supports stackable items, hotbar slots, active slot selection, drag/drop-friendly slot interfaces, item dropping into the world, and JSON-friendly save/load reconstruction using item IDs from the `ItemDatabase`.

---

## 🛑 The Challenge (The "Problem")

A common beginner approach is to let UI slots store the actual items directly. This seems simple at first, but creates problems when saving, loading, dropping items, or changing the UI layout.

- **Complexity:** If the item data lives inside UI buttons or images, the inventory becomes tied to the current menu layout instead of being a real data system.
- **Dependencies:** The inventory data should not depend on UI objects, and UI slots should not be responsible for deciding how items stack, save, or drop into the world.
- **Scalability:** Adding hotbars, chests, equipment slots, drag-and-drop, or save/load would become much harder if every UI slot managed its own item behavior independently.

---

## 🏗 The Architecture (The "Solution")

The system separates inventory responsibilities into focused layers:

* **`InventoryManager`**: The source of truth for inventory data. It owns the `ItemInstance[]` array, handles stacking, removing, swapping, dropping, active slot changes, and save/load logic.
* **`InventoryUI`**: Builds the visual inventory and hotbar slots, subscribes to inventory events, and refreshes only the slots that changed.
* **`InventorySlotUI`**: Displays a single slot by reading from `InventoryManager`, showing item name, icon, stack size, and animated icons when needed.
* **`IStorageSlot`**: Provides a shared interface for storage slot UI behavior, making the system easier to expand for drag-and-drop or other storage types.

### 🧩 Patterns & Principles Used:

* **Observer Pattern:** `InventoryManager` raises events like `OnSlotUpdated` and `OnActiveSlotIndexChanged`, allowing UI to react without being tightly coupled to inventory logic.
* **Single Responsibility Principle:** Data management, UI spawning, and individual slot rendering are handled by separate classes.
* **Interface-Driven Design:** `IStorageSlot` gives slot UI elements a shared contract for refreshing and drag state.
* **Data-Driven Save/Load:** Inventory saves lightweight slot data using index, item ID, and stack size, then rebuilds `ItemInstance` objects through the `ItemDatabase`.
* **Separation of Concerns:** The UI displays inventory state, but the manager owns the inventory state.

---

## 💻 Code Highlight

This method is the core inventory insertion logic. It first tries to merge stackable items into existing stacks, including partial stack fills, before falling back to the first empty slot.

A strong design choice is that the method only notifies the UI through events. This allows each component to remain decoupled, and also allows other future systems to easily detect when changes occur in the `Inventory`

The inventory manager only changes the data and announces which slot changed.

```csharp
public bool AddItems(ItemInstance item)
{
    // Try to stack first if the item is stackable
    if (item.DataSo.IsStackable == true)
    { 
        for (int i = 0; i < itemsList.Length; i++)
        {
            // Skip empty slots and slots with different items
            if (itemsList[i] == null || itemsList[i].DataSo != item.DataSo) continue;
            
            int spaceLeft = itemsList[i].DataSo.MaxStackSize - itemsList[i].stackSize;
            
            // Skip full slots
            if (spaceLeft <= 0) continue;

            // If whole new stack fits in current slot
            if (item.stackSize <= spaceLeft)
            {
                itemsList[i].stackSize += item.stackSize;
                OnSlotUpdated?.Invoke(i);
                OnItemAddedToInventory?.Invoke(item);
                return true;
            }
            // If partial new stack fits in current slow
            else
            {
                itemsList[i].stackSize = itemsList[i].DataSo.MaxStackSize;
                item.stackSize -= spaceLeft;
                OnSlotUpdated?.Invoke(i);
                // Do not return true yet, code continues to find open slot for remainder
            }
        }
    }
    
    // If not stackable / No free stack available
    for (int i = 0; i < itemsList.Length; i++)
    {
        if (itemsList[i] == null)
        {
            itemsList[i] = item; // Adds the item
            OnSlotUpdated?.Invoke(i);
            OnItemAddedToInventory?.Invoke(item);
            return true;
        }
    }

    Debug.unityLogger.Log("Iventory is full");
    return false; // If inventory is full
}
```

