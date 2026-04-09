using UnityEngine;

public enum DamageType
{
    Physical,
    Fire,
    Water,
    Earth,
    Lightning,
    Holy,
    Shadow
}

[System.Serializable]
public struct DamageData
{
    public float damageAmount;
    public Vector2 hitDirection;
    public float knockbackForce;
    public DamageType type;
    public GameObject source;
    
    // Constructor to easily create damage types on the fly
    public DamageData(float amount, Vector2 direction, float knockback, DamageType dmgType, GameObject from = null)
    {
        damageAmount = amount;
        hitDirection = direction;
        knockbackForce = knockback;
        type = dmgType;
        source = from;
    }
}
