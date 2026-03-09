using UnityEngine;

public class InventoryController : MonoBehaviour
{
    [Header("UI Settings")]
    public GameObject inventoryPanel; // The Grid/Panel that holds the slots
    public GameObject slotPrefab; 
    
    [Header("Inventory Setup")]
    public int slotCount; // How many total slots to create?
    public GameObject[] itemPrefabs; // List of items to put in the slots at the start

    void Start()
    {
        //Create the inventory grid of slots based on slotCount
        for (int i = 0; i < slotCount; i++)
        {
            // 1. Create a new slot prefab and make it a child of GameObject inventoryPanel
            //The layout group in inventoryPanel organizes the slots automatically
            GameObject slotObject = Instantiate(slotPrefab, inventoryPanel.transform);
            
            // 2. Grab the Slot component from the Slot Prefab 
            Slot slot = slotObject.GetComponent<Slot>();
            
            // 3. Check: Do we have an item in our itemPrefabs for this specific slot index (i)
            if (i < itemPrefabs.Length)
            {
                // 4. Create the item and make it a child of the slot
                GameObject item = Instantiate(itemPrefabs[i], slot.transform);
                
                // 5. Center the item inside the slot
                // Using anchoredPosition = Vector2.Zero puts it exavtly in the middle of the box
                item.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                
                // 6. Pass the item details to the Slot
                slot.currentItem = item;
            }
        }
    }
}
