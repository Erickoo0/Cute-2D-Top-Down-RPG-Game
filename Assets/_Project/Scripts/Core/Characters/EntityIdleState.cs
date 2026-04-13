using UnityEngine;

public class EntityIdleState : State<EntityController>
{
    public EntityIdleState(EntityController context, StateMachine stateMachine) : base(context, stateMachine) { }

    private float idleTime;
    
    public override void Enter()
    {
        context.EntityMover.SetMoveDirection(Vector2.zero); 
        idleTime = Random.Range(1f, 3f);
    }

    public override void Update()
    {
        if (context.IsTargetInRange())
        {
            //stateMachine.ChangeState(context.ChaseState);
        }
    }
    
    public override void PhysicsUpdate() 
    {
        if (idleTime > 0) idleTime--;
        else stateMachine.ChangeState(context.WanderState);
    }
    public override void Exit() { }
}
