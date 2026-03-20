using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(SpriteRenderer), typeof(BoxCollider2D))]
public class ItemObject : MonoBehaviour
{
    [Header("Settings")] 
    [SerializeField] private ItemData startingItemData; // Used ONLY when placing items manually via the editor

    [Header("Spawn Animation")]
    [SerializeField] private float bounceDuration = 0.5f;
    [SerializeField] private float bounceHeight = 0.65f;
    [SerializeField] private int bounceCount = 3;
    
    private ItemInstance _itemInstance;
    private SpriteRenderer _spriteRenderer;
    private bool _canBePickedUp = false;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        
        // If we assigned starting item in the inspector, then initialize it
        if (startingItemData != null) InitializeItem(new ItemInstance(startingItemData));
        
    }
    
    public void InitializeItem(ItemInstance newItemInstance)
    {
        _itemInstance = newItemInstance;
        _spriteRenderer.sprite = _itemInstance.Data.itemIcon;
        gameObject.name = _itemInstance.Data.itemName;

        PlaySpawnAnimation();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!_canBePickedUp) return;
        TryPickup(collision);
    }
    
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!_canBePickedUp) return;
        TryPickup(collision);
    }

    private void PlaySpawnAnimation()
    {
        _canBePickedUp = false;
        
        Vector3 startPosition = transform.position; //Save the initial position

        // Create a bounce sequence using DOTween (Group multiple tweens together)
        Sequence bounceSequence = DOTween.Sequence();
        
        for (int i = 0; i < bounceCount; i++)
        {
            // Decrease the height and duration each bounce
            float currentBounceHeight = bounceHeight * (1f - (i * 0.4f));
            float currentDuration = bounceDuration * (1f - (i * 0.2f));
        
            // Control bounce up
            bounceSequence.Append(transform.DOMoveY(startPosition.y + currentBounceHeight, currentDuration / 2)
                .SetEase(Ease.OutQuad));
            // Control bounce down
            bounceSequence.Append(transform.DOMoveY(startPosition.y, currentDuration / 2)
                .SetEase(Ease.InQuad));
        }
        
        bounceSequence.OnComplete(() => _canBePickedUp = true);
    }

    private void TryPickup(Collider2D collision)
    {
        if (collision.CompareTag("Player") && _itemInstance != null)
        {
            _canBePickedUp = false; // Set to false since item is now picked up
            // Tell InventoryManager to add item to inventory
            bool wasPickedUp = InventoryManager.Instance.AddItems(_itemInstance);
            if (wasPickedUp)
            {
                // Add other effects here like sound later
                Destroy(gameObject);
            }
        }
    }
}
