using UnityEngine;

public class ItemPopupManager : MonoBehaviour
{
    [Header("Visual References")]
    public GameObject itemPopupUI;
    public Transform popupParent; // Vertical Layout Group

    private void Start()
    {
        InventoryManager.Instance.OnItemAddedToInventory += SpawnPopup;
    }

    private void SpawnPopup(ItemInstance itemInstance)
    {
        GameObject newPopup = Instantiate(itemPopupUI, popupParent);
        newPopup.GetComponent<ItemPopup>().SetPopUp(itemInstance);
    }
    
    private void OnDestroy()
    {
        if (InventoryManager.Instance != null)
            InventoryManager.Instance.OnItemAddedToInventory -= SpawnPopup;
    }
}
