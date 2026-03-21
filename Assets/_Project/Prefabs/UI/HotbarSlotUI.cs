using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HotbarSlotUI : MonoBehaviour, IStorageSlot
{
    public int slotScriptIndex;
    public int Index => slotScriptIndex;
    public ItemInstance itemInstance => InventoryManager.Instance.itemsList[slotScriptIndex];

    [Header("UI References")]
    [SerializeField] private Image itemIconDisplay;
    [SerializeField] private TextMeshProUGUI itemNameText;
    [SerializeField] private TextMeshProUGUI itemStackText;

    private bool _isBeingDragged = false;

    private void Update()
    {
        // Only run this if the item is valid, animated, and not hidden by dragging
        if (itemInstance?.Data != null && itemInstance.Data.animated && !_isBeingDragged)
        {
            itemIconDisplay.sprite = GlobalHelper.GetAnimatedSprite(itemInstance.Data);
        }
    }
    
    public void RefreshSlotUI()
    {
        var item = itemInstance; // Gets data from Inventory Manager via Property
        bool hasItem = item != null && item.Data != null; // Check if the slot has an item
        bool shouldShow = hasItem && !_isBeingDragged; // Hides elements while being dragged

        // Set the text
        itemNameText.text = hasItem ? item.Data.itemName : null;
        itemStackText.text = hasItem ? item.stackSize.ToString() : null;
        
        // If not animated, set the sprite
        if (hasItem && !itemInstance.Data.animated)
            itemIconDisplay.sprite = item.Data.itemIconAnimated[0];
        
        
        itemIconDisplay.enabled = shouldShow;
    }

    public void SetDraggingState(bool isDragging)
    {
        _isBeingDragged = isDragging;
        RefreshSlotUI();
    }
}