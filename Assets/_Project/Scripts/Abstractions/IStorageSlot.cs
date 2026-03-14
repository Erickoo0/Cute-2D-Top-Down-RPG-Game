using UnityEngine;

public interface IStorageSlot
{
    ItemInstance Item { get; } // Pulls data from Inventory Manager
    int Index { get; }
    void RefreshUI(); 
    void SetDraggingState(bool isDragging);
}
