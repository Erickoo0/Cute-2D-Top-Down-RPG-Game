# 🖱️ Technical Deep Dive: Drag and Drop System

## **Overview**
The Drag and Drop system provides a high-performance, centralized way to move items between slots, swap positions, or drop items into the 2D world. 
It is designed to be flexible and can interact with any UI element that implements the `IStorageSlot` interface. This makes it easy to drag items
from the **inventory** to the **shop** or the **chest** in the future. 



---

## **1. The DragManager (Centralized Controller)**
Instead of putting drag logic on every individual slot (which is computationally expensive and hard to maintain), I implemented a **Centralized Manager**.

### **Key Logic Flow:**
1.  **Start Drag:** On mouse click, the manager performs a **Graphic Raycast** to detect if an `IStorageSlot` is under the cursor.
2.  **The Ghost Icon:** If an item is found, the Manager enables a "Ghost Icon"—a temporary UI element that takes information from `IStorageSlot` and follows the mouse position, providing immediate visual feedback.
3.  **The Source Slot:** The original slot is told to hide its UI elements via interface methods so the item appears to have been "picked up."
4.  **End Drag:** On release, a final raycast determines the destination.

---

## **2. Interaction Logic & Physics**
The system distinguishes between **UI Swapping** and **World Dropping**:

* **UI Swapping:** If the mouse is released over another `IStorageSlot`, the Manager calls `InventoryManager.Instance.SwapItems(sourceIndex, targetIndex)`.
   * **Atomic Swapping:** Utilized modern C# Tuple deconstruction for the swap logic:
    ` (itemsList[indexA], itemsList[indexB]) = (itemsList[indexB], itemsList[indexA]); `
* **World Dropping:** If the mouse is released over the game world (no UI hit), the system converts the screen coordinates to world coordinates 
using `Camera.ScreenToWorldPoint` and triggers `InventoryManager.Instance.DropItems()`.

---

## **3. Decoupling via Interfaces**
By using the `IStorageSlot` interface, the **DragManager** never needs to know if it's dragging from a Player Inventory, a Bank, or a Shop. 
