using UnityEngine;
using System.Linq;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "ItemDatabase", menuName = "Inventory/Database")]
public class ItemDatabase : ScriptableObject
{
    [Tooltip("Add every ItemData in the game here")]
    public List<ItemData> allItems;
    
    private Dictionary<string, ItemData> _itemDictionary;

    /// <summary>
    /// Builds the dictionary, called once when loading a save file
    /// </summary>
    public void Initialize()
    {
        _itemDictionary = new Dictionary<string, ItemData>();

        foreach (var item in allItems)
        {
            if (item != null && !string.IsNullOrEmpty(item.itemID))
            {
                if (!_itemDictionary.ContainsKey(item.itemID))
                {
                    _itemDictionary.Add(item.itemID, item);
                }
                else
                {
                    Debug.LogWarning($"[ItemDatabase] Duplicate Item ID found: {item.itemID}. IDs must be unique!");
                }
            }
        }
    }

    public ItemData GetItem(string itemID)
    {
        if (_itemDictionary == null) Initialize();
        return _itemDictionary.TryGetValue(itemID, out ItemData item) ? item : null;
    }
}
