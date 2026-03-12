using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Slot : MonoBehaviour, IStorageSlot
{
    public int slotScriptIndex;
    public ItemInstance itemInstance;
    
    public ItemInstance Item => itemInstance;
    public int Index => slotScriptIndex;

    [SerializeField] private Image itemIconDisplay;
    [SerializeField] private TextMeshProUGUI itemNameText;
    [SerializeField] private TextMeshProUGUI itemStackText;

    public void UpdateSlot(ItemInstance newItem)
    {
        itemInstance = newItem;
        bool hasItem = itemInstance != null && itemInstance.Data != null;

        if (hasItem)
        {
            // 1. Update Visuals
            itemIconDisplay.sprite = itemInstance.Data.itemIcon;
            itemIconDisplay.enabled = true;

            // 2. Update Text Info
            itemNameText.text = itemInstance.Data.itemName;

            // 3. Handle Stack Counter
            if (itemInstance.Data.isStackable && itemInstance.stackSize > 1)
            {
                itemStackText.text = itemInstance.stackSize.ToString();
                itemStackText.enabled = true;
            }
            else
            {
                itemStackText.enabled = false;
            }
        }
        else
        {
            // Reset/Clear everything if there is no item
            itemIconDisplay.sprite = null;
            itemIconDisplay.enabled = false;
        
            itemNameText.text = "";
            itemStackText.enabled = false;
        }
    }

    public void SetIconVisibility(bool state)
    {
        // If the slot is empty, we don't want the icon enabled anyway
        if (itemInstance == null) 
        {
            itemIconDisplay.enabled = false;
            return;
        }

        itemIconDisplay.enabled = state;
    }
}
