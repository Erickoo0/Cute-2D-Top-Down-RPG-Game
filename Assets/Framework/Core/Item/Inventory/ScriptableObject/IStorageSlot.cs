using UnityEngine;

public interface IStorageSlot
{
    ItemInstance itemInstance { get; } // Pulls data from Inventory Manager
    int Index { get; }
    void RefreshSlotUI(); 
    void SetDraggingState(bool isDragging);
}
