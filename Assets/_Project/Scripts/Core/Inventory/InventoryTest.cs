using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryTest : MonoBehaviour
{
    [Header("Input Settings")]
    [Tooltip("Bind the key or button you want to use to spawn items.")]
    public InputAction addItemHotkey;

    private void OnEnable() => addItemHotkey.Enable();
    private void OnDisable() => addItemHotkey.Disable();
    
    void Update()
    {
        // Wait for spacebar
        if (!addItemHotkey.WasPressedThisFrame()) return;
        
        // Grab the database directly Inventory Manager
        ItemDatabase db = InventoryManager.Instance.itemDatabase;
        
        if (db == null || db.allItems.Count == 0) return;

        // Pick a random item
        ItemData randomItemData = db.allItems[Random.Range(0, db.allItems.Count)];
        
        // Determine amount and create instance
        int randomAmount = randomItemData.isStackable ? Random.Range(1, 6) : 1;
        ItemInstance newItemInstance = new ItemInstance(randomItemData, randomAmount);
        
        // Add to inventory
        bool itemAdded = InventoryManager.Instance.AddItems(newItemInstance);
        
        if (itemAdded) 
            Debug.unityLogger.Log($"Added {randomAmount}x {randomItemData.itemName}!");
        else 
            Debug.unityLogger.Log("Inventory is full!");
    }
}