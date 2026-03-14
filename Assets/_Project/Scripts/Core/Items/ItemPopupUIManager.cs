using UnityEngine;

public class UI_itemPopup_Manager : MonoBehaviour
{
    [Header("Visual References")]
    public GameObject itemPopupPrefab;
    public Transform popupParent; // Vertical Layout Group

    private void Start()
    {
        InventoryManager.Instance.OnItemAddedToInventory += SpawnPopup;
    }

    private void SpawnPopup(ItemInstance itemInstance)
    {
        GameObject newPopup = Instantiate(itemPopupPrefab, popupParent);
        newPopup.GetComponent<ItemPopup>().PopupSetup(itemInstance);
    }
    
    private void OnDestroy()
    {
        if (InventoryManager.Instance != null)
            InventoryManager.Instance.OnItemAddedToInventory -= SpawnPopup;
    }
}
