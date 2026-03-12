using System;
using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Manages the global player inventory using a Singleton pattern. 
/// Handles item storage, addition, and swapping, while notifying listeners of slot changes via events.
/// </summary>
public class InventoryManager : MonoBehaviour
{
    // Singleton Pattern: Accessible from any script
    public static InventoryManager Instance { get; private set; }

    // Observer Pattern: This event signals a slot has changed and displays the slot index number
    public event Action<int> OnSlotUpdated;

    [Header("Inventory Settings")]
    [SerializeField] private int inventorySize = 20;
    

    // An empty array of items 
    public ItemInstance[] itemsList;

    private void Awake()
    {
        // Singleton Pattern: Safety measure to prevent new additional InventoryManagers from being created
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this; // Assign This ID to variable

        InitializeInventory();
    }

    /// <summary>
    /// Initializes an empty array to hold our items.
    /// We do it here in case we change inventorySize in a save file later
    /// </summary>
    private void InitializeInventory()
    {
        itemsList = new ItemInstance[inventorySize];
    }
    
    public bool AddItems(ItemInstance item)
    {
        // Try to stack first if the item is stackable
        if (item.Data.isStackable == true)
        { 
            for (int i = 0; i < itemsList.Length; i++)
            {
                // Find the same item in inventory
                if (itemsList[i] != null && itemsList[i].Data == item.Data)
                {
                    // Check if the stack has room
                    if (itemsList[i].stackSize < itemsList[i].Data.maxStackSize)
                    {
                        itemsList[i].stackSize += item.stackSize;
                        //If we exceed max, we can split remainder, but for now, keeping it simple
                        OnSlotUpdated?.Invoke(i);
                        return true;
                    }
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
                return true;
            }
        }

        return false; // If itemsList is full
    }
    
    public void SwapItems(int indexA, int indexB)
    {
        // Check if indexes are out of range of itemsList array
        if (indexA < 0 || indexA >= itemsList.Length || indexB < 0 || indexB >= itemsList.Length) return;

        // Swaps item from A and B using modern C# deconstruction
        (itemsList[indexA], itemsList[indexB]) = (itemsList[indexB], itemsList[indexA]);

        // Trigger event for BOTH slots involved in the swap
        OnSlotUpdated?.Invoke(indexA);
        OnSlotUpdated?.Invoke(indexB);
    }

    public void DropItems(int index, Vector3 spawnPosition)
    {
        // Safety Check: Make sure the slot is not already empty
        if (itemsList[index] == null) return;
        
        // Spawn the item
        GameObject droppedItem = Instantiate(itemsList[index].Data.itemObject, spawnPosition,  Quaternion.identity );
        if (droppedItem.TryGetComponent(out ItemObject itemObject))
        {
            itemObject.InitializeItem(itemsList[index]);
        }
        
        // Clear the slot and notify the UI
        itemsList[index] = null;
        OnSlotUpdated?.Invoke(index);
    }

    /// <summary>
    /// Packages current inventory into lightweight structs for saving
    /// </summary>
    public List<SavedSlot> GetInventorySaveData()
    {
        List<SavedSlot> savedSlots = new List<SavedSlot>();

        for (int i = 0; i < itemsList.Length; i++)
        {
            if (itemsList[i] != null && itemsList[i].Data != null)
            {
                savedSlots.Add(new SavedSlot
                {
                    index = i,
                    itemID = itemsList[i].Data.itemID,
                    itemStackSize = itemsList[i].stackSize
                });
            }
        }
        return savedSlots;
    }

    /// <summary>
    /// Rebuilds the inventory from saved data using the ItemDatabase.
    /// </summary>
    public void LoadInventoryData(List<SavedSlot> savedData, ItemDatabase database)
    {
        // 1. Clear current inventory
        InitializeInventory(); 

        // 2. Rebuild instances
        foreach (var savedSlot in savedData)
        {
            // Safety check: ensure index is within bounds
            if (savedSlot.index >= 0 && savedSlot.index < inventorySize)
            {
                ItemData data = database.GetItem(savedSlot.itemID);
                
                if (data != null)
                {
                    // Recreate the unique instance!
                    itemsList[savedSlot.index] = new ItemInstance(data, savedSlot.itemStackSize);
                }
                else
                {
                    Debug.LogWarning($"Could not find item with ID: {savedSlot.itemID} in database.");
                }
            }
        }

        // 3. Notify the UI to refresh ALL slots
        for (int i = 0; i < itemsList.Length; i++)
        {
            OnSlotUpdated?.Invoke(i);
        }
    }
}
