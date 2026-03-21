using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryTest : MonoBehaviour
{
    [Header("Input Settings")]
    [Tooltip("Bind the key or button you want to use to spawn items.")]
    public InputAction addItemHotkey;

    private GameObject _player;

    private void OnEnable() => addItemHotkey.Enable();
    private void OnDisable() => addItemHotkey.Disable();
    
    private void Start() => _player = GameObject.FindGameObjectWithTag("Player");
    
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
        //GameObject droppedItemObj = Instantiate(newItemInstance.Data.itemObject, _player.transform , Quaternion.identity);

        bool itemAdded = InventoryManager.Instance.AddItems(newItemInstance);
        
        if (itemAdded) 
            Debug.unityLogger.Log($"Added {randomAmount}x {randomItemData.itemName}!");
        else 
            Debug.unityLogger.Log("Inventory is full!");
    }
}