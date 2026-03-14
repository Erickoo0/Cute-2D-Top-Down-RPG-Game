using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Slot : MonoBehaviour, IStorageSlot
{
    public int slotScriptIndex;
    public ItemInstance itemInstance;
    
    public ItemInstance Item => itemInstance;
    public int Index => slotScriptIndex;

    [Header("UI References")]
    [SerializeField] private Image itemIconDisplay;
    [SerializeField] private TextMeshProUGUI itemNameText;
    [SerializeField] private TextMeshProUGUI itemStackText;

    public void UpdateSlot(ItemInstance newItem)
    {
        itemInstance = newItem;
        // Refresh the UI state immediately when data changes
        SetVisibility(true);
    }

    public void SetVisibility(bool toggle)
    {
        UpdateSlotUI(toggle);
    }
    
    private void UpdateSlotUI(bool toggle)
    {
        // Check if slot has item
        bool hasItem = itemInstance != null && itemInstance.Data != null;
        
        // We only show if the toggle is on AND we have an item.
        bool showElements = toggle && hasItem;

        // Updates Icon if slot has an item, and shows icon only if toggled
        if (itemIconDisplay != null) 
        {
            itemIconDisplay.sprite = hasItem ? itemInstance.Data.itemIcon : null;
            itemIconDisplay.enabled = showElements;
        }

        // Updates Name if slot has an item, and shows Name only if toggled
        if (itemNameText != null) 
        {
            itemNameText.text = hasItem ? itemInstance.Data.itemName : "";
            itemNameText.enabled = showElements;
        }

        // Updates Stack if slot has an item, and shows stack only if toggled
        if (itemStackText != null)
        {
            bool shouldShowStack = showElements && 
                                   itemInstance.Data.isStackable && 
                                   itemInstance.stackSize > 1;
                             
            itemStackText.text = shouldShowStack ? itemInstance.stackSize.ToString() : "";
            itemStackText.enabled = shouldShowStack;
        }
    }
}