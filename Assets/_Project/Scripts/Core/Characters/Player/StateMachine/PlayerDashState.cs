using UnityEngine;
public class PlayerDashState : State<PlayerController>
{
    public PlayerDashState(PlayerController context, StateMachine stateMachine) : base(context, stateMachine) { }

    private float dashTime;
    private float defaultMoveSpeed;
    public override void Enter()
    {
        dashTime = 1f;
        defaultMoveSpeed = context.EntityMover.moveSpeed;
        context.EntityMover.moveSpeed = context.EntityMover.moveSpeed * 2f;
    }
    public override void Update()
    {
        Vector2 input = context.MovementInput;
        
        context.EntityMover.SetMoveDirection(input);
    }

    public override void PhysicsUpdate()
    {
        if (dashTime > 0) dashTime--;
        else stateMachine.ChangeState(context.IdleState);
    }

    public override void Exit()
    {
        context.EntityMover.moveSpeed = defaultMoveSpeed; 
    }
}
