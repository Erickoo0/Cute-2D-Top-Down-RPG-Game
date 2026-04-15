using UnityEngine;

public class EnemyController : EntityController
{
    [Header("AI Settings")] 
    public float aggroRange = 5f;
    public Transform target;
}
