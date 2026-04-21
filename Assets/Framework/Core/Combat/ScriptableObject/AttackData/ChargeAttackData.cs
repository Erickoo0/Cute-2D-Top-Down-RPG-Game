using UnityEngine;

[CreateAssetMenu(fileName = "ChargeAttackData", menuName = "AttackData/ChargeAttackData")]
public class ChargeAttackData : AttackData
{
    [Header("Movement Settings")] 
    public float windUpDuration = 0.75f;
    public float chargeSpeedMultiplier = 4f;
    public float overshootDistance = 5f;
}
