using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SlotUI : MonoBehaviour, IStorageSlot
{
    public int slotScriptIndex;
    public int Index => slotScriptIndex;
    public ItemInstance Item => InventoryManager.Instance.itemsList[slotScriptIndex];

    [Header("UI References")]
    [SerializeField] private Image itemIconDisplay;
    [SerializeField] private TextMeshProUGUI itemNameText;
    [SerializeField] private TextMeshProUGUI itemStackText;

    private bool _isBeingDragged = false;

    public void RefreshUI()
    {
        var item = Item; // Gets data from Inventory Manager via Property
        bool hasItem = item != null && item.Data != null; // Check if the slot has an item
        
        bool shouldShow = hasItem && !_isBeingDragged; // Hides elements while being dragged

        itemIconDisplay.sprite = hasItem ? item.Data.itemIcon : null;
        itemIconDisplay.enabled = shouldShow;
        
        itemNameText.text = hasItem ? item.Data.itemName : null;
        itemNameText.enabled = shouldShow;

        itemStackText.text = hasItem ? item.stackSize.ToString() : null;
        itemStackText.enabled = shouldShow;
    }

    public void SetDraggingState(bool isDragging)
    {
        _isBeingDragged = isDragging;
        RefreshUI();
    }
}