using UnityEngine;

/// <summary>
/// Attached to every Slot prefab to keep track of the item it contains
/// </summary>
public class Slot : MonoBehaviour
{
    // Item currently held in slot
    public GameObject currentItem;
}
