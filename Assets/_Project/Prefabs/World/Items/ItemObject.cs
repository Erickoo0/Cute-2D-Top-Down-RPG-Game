using UnityEngine;

[RequireComponent(typeof(SpriteRenderer), typeof(BoxCollider2D))]
public class ItemObject : MonoBehaviour
{
    [Header("Settings")] 
    [SerializeField] private ItemData itemData; // Leave blank unless we want to manually add item to the world at the start of game
    
    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        
        // If we assigned itemData in the inspector, then initialize it
        if (itemData != null) InitializeItem(itemData);
    }

    /// <summary>
    /// Updates the <see cref="ItemObject"/> data with the new item data
    /// </summary>
    public void InitializeItem(ItemData newItemData)
    {
        itemData = newItemData;
        _spriteRenderer.sprite = itemData.itemIcon;
        gameObject.name = itemData.itemName;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            bool wasPickedUp = InventoryManager.Instance.AddItems(itemData);
            
            if (wasPickedUp)
            {
                // Add other effects here like sound later
                Destroy(gameObject);
            }
        }
    }
}
