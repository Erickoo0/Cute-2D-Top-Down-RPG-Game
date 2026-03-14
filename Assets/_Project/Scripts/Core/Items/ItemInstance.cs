/// <summary>
/// The Identity: Dynamic item data, unique to each instance
/// </summary>
[System.Serializable]
public class ItemInstance
{
    public ItemData Data { get; private set; }
    public int stackSize; // Tracks current amount in this instance
    
    //future properties will go here

    public ItemInstance(ItemData itemData, int amount = 1)
    {
        Data = itemData;
        stackSize = amount;
    }
}
