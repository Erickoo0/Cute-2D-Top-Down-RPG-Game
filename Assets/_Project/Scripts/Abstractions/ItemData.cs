using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class ItemData : ScriptableObject
{
    [Header("Item Data")]
    public string itemName;
    public Sprite itemIcon;
    public GameObject itemObject; // The physical item that gets picked up
    [TextArea] public string itemDescription;
}
