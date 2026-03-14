using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Serialization;

public class ItemPopup : MonoBehaviour
{
    public Image itemIcon;
    public TextMeshProUGUI itemName;
    public TextMeshProUGUI itemStackSize;

    [SerializeField] private float lifetime = 3f;

    public void PopupSetup(ItemInstance itemInstance)
    {
        itemIcon.sprite = itemInstance.Data.itemIcon;
        itemName.text = itemInstance.Data.itemName;
        itemStackSize.text = itemInstance.stackSize.ToString();
        
        Destroy(gameObject, lifetime);
    }
}
