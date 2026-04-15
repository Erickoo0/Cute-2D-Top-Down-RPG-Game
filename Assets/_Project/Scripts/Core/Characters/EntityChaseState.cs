using UnityEngine;

public class EntityChaseState : State<EntityController>
{
    public EntityChaseState(EntityController context, StateMachine stateMachine) : base(context, stateMachine) { }
    
    public override void Enter() { }

    public override void Update()
    {
        if (context.currentTarget == null)
        {
            stateMachine.ChangeState(context.IdleState);
            return;
        }
        
        Vector2 currentPosition = context.transform.position;
        Vector2 targetPosition = context.currentTarget.position;
        
        float distance = Vector2.Distance(currentPosition, targetPosition);
        
        // If in attack range and action cooldown is over, perform action
        if (distance <= context.actionRange && context.CanPerformAction())
        {
            stateMachine.ChangeState(context.ActionState);
        }
        else // Keep Chasing
        {
            Vector2 moveDirection = (targetPosition - currentPosition).normalized;
            context.EntityMover.SetMoveDirection(moveDirection);
        }
    }
    
    public override void PhysicsUpdate() { }
    public override void HandleInput() { }

    public override void Exit()
    {
        // Stop moving when leaving chase state so entity does not slide when transitioning
        context.EntityMover.SetMoveDirection(Vector2.zero);
    }
}
