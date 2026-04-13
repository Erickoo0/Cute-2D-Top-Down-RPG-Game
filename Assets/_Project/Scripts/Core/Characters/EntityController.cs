using UnityEngine;
using System.Collections.Generic;

public class EntityController : BaseEntityController
{
    [Header("AI Settings")] 
    public float aggroRange = 5f;
    public Transform target;
    
    [Header("Movement Data")]
    public Transform[] waypointsList;
    public int currentWaypointIndex = 0;
    public float waypointWaitTime = 2f;
    public bool loopWaypoints = true;
    
    [Header("State References")]
    public EntityIdleState  IdleState { get; private set; }
    public EntityWanderState WanderState { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        IdleState = new EntityIdleState(this, StateMachine);
        WanderState = new EntityWanderState(this, StateMachine);
    }

    protected virtual void Start()
    {
        // Start by wandering to first waypoint
        StateMachine.SetupState(WanderState);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        
        if (target == null) target = other.transform;
        
        Vector2 lookDirection = (other.transform.position - transform.position).normalized;    
        EntityAnimator.FaceDirection(lookDirection);
        
        // If wandering, force idle state
        if (StateMachine.CurrentState == WanderState) StateMachine.ChangeState(IdleState);
    }

    //---- Helper Methods ----
    public bool IsTargetInRange()
    {
        if (target is null) return false;
        return Vector2.Distance(transform.position, target.position) <= aggroRange;
    }

    public Vector2 GetCurrentWaypointPosition()
    {
        if (waypointsList == null || waypointsList.Length <= 0) return transform.position;
        return waypointsList[currentWaypointIndex].position;
    }

    public void AdvanceToNextWaypoint()
    {
        if (waypointsList == null || waypointsList.Length <= 0) return;
        
        if (loopWaypoints) currentWaypointIndex = (currentWaypointIndex + 1) % waypointsList.Length;
        else if (currentWaypointIndex < waypointsList.Length - 1) currentWaypointIndex++;
    }
}