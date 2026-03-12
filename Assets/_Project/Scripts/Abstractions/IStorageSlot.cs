using UnityEngine;

public interface IStorageSlot
{
    ItemInstance Item { get; }
    int Index { get; }
    void UpdateSlot(ItemInstance newItem);
    void SetIconVisibility(bool state);
}
