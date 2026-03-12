using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// A test script that adds item instances to inventory from a pool of <see cred="ItemData"/>
/// </summary>
public class InventoryTest : MonoBehaviour
{
    [Header("Item Data Pool")]
    public ItemData[] testItemPool;

    void Update()
    {
        if (testItemPool == null || testItemPool.Length == 0) return;
        
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            
           ItemData selectedItem = SelectRandomItem();
           int randomAmount = selectedItem.isStackable ? Random.Range(1, 6) : 1;
           
           // Create unique instance from itemData
           ItemInstance newItemInstance = new ItemInstance(selectedItem, randomAmount);
            
           // Add the instance
           bool ifItemAdded = InventoryManager.Instance.AddItems(newItemInstance);
           
           if (ifItemAdded) 
               Debug.unityLogger.Log($"Item {selectedItem.itemName} added to inventory");
           else 
               Debug.unityLogger.Log($"Inventory is full! Cannot add more items");
        }
    }

    private ItemData SelectRandomItem()
    {
        int randomIndex = Random.Range(0, testItemPool.Length);
        ItemData selectedItem = testItemPool[randomIndex];
        return selectedItem;
    }
}
