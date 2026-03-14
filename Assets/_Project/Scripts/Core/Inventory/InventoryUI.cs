using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Manages the visual representation of the player's inventory. 
/// Spawns <see cref="SlotUI"/> elements and listens for data changes to refresh specific slots.
/// </summary>
public class InventoryUI : MonoBehaviour
{
    public GameObject slotPrefab; // The Slots to spawn
    public Transform slotParent; // The Grid Layout Group
    
    // Keep a list of Slot scripts we created
    private readonly List<IStorageSlot> _slots = new List<IStorageSlot>();

    private void Start()
    {
        SetupUI();
        
        // Subscribe to InventoryManager.cs event
        InventoryManager.Instance.OnSlotUpdated += RefreshSlotUI;
        
        // Initial Refresh: Sync UI with whatever data is already in the Inventory Manager (Saved data)
        for (int i = 0; i < InventoryManager.Instance.itemsList.Length; i++)
        {
            RefreshSlotUI(i);
        }
    }

    private void OnDestroy()
    {
        // Ubsubscribe when destroyed to prevent memory leaks
        if (InventoryManager.Instance != null)
        {
            InventoryManager.Instance.OnSlotUpdated -= RefreshSlotUI;
        }
    }

    private void SetupUI()
    {
        
        for (int i = 0; i < InventoryManager.Instance.itemsList.Length; i++)
        {
            GameObject slot = Instantiate(slotPrefab, slotParent); // Creates Slot Prefab
            if (slot.TryGetComponent(out SlotUI storageSlot))
            {
                storageSlot.slotScriptIndex = i;
                _slots.Add(storageSlot);
            }
        }
    }

    private void RefreshSlotUI(int index)
    {
        if (index >= 0 && index < _slots.Count)
        {
            _slots[index].RefreshUI();
        }
    }
}