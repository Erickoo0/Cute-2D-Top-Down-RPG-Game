using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class SaveData
{
    public Vector3 playerPosition;
    public string mapBoundaryName; //Stores Boundary Name
    
    public List<SavedSlot> inventoryData = new List<SavedSlot>();
}


[System.Serializable]
public struct SavedSlot
{
    public int index; // Which Slot Index?
    public string itemID; 
    public int itemStackSize;
}