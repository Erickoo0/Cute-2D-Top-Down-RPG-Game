using UnityEngine;

[ CreateAssetMenu(fileName = "New Consumable Item", menuName = "Items/Consumable")]
public class ItemDataConsumable : ItemData
{
    [Header("Consumable Effects")]
    public int healthHealAmount = 10;
    public int healthHealCount = 1;
    public int healthHealDurationTick = 2;
    public int manaHealAmount = 0;
    public int manaHealCount = 1;
    public int manaHealDurationTick = 2;

    public override bool Use(GameObject user, ItemInstance itemInstance)
    {
        if (healthHealAmount > 0)
        {
            var target = user.GetComponent<Health>();
            
            target.HealthCurrent += healthHealAmount;
            return true;
        }
        else
        {
            return false;
        }
    }
}
