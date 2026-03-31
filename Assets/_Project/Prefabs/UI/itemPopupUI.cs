using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Serialization;

public class ItemPopup : MonoBehaviour
{
    public ItemInstance itemInstance;
    public Image itemIconDisplay;
    public TextMeshProUGUI itemName;
    public TextMeshProUGUI itemStackSize;

    [SerializeField] private float lifetime = 3f;

    private void Update()
    {
        if (itemInstance?.Data == null || !itemInstance.Data.IsAnimated)
            return;
        
        itemIconDisplay.sprite = GlobalHelper.GetAnimatedSprite(itemInstance.Data);
    }
    
    public void SetPopUp(ItemInstance newItem)
    {
        itemInstance = newItem;
        
        // Set the text
        itemName.text = itemInstance.Data.ItemName;
        itemStackSize.text = itemInstance.stackSize.ToString();
 
        Destroy(gameObject, lifetime);
    }
}
