using UnityEngine;

public class EntityActionState : State<EntityController>
{
    public EntityActionState(EntityController context, StateMachine stateMachine) : base(context, stateMachine) { }

    private float _windUpTimer;
    private float _chargeTimer;
    private Vector2 _chargeDirection;
    
    private float _windUpDuration = 0.75f; // Pause for 0.75 seconds
    private float _chargeDuration = 0.4f;  // Dash for 0.4 seconds
    private float _chargeSpeedMultiplier = 4f;
    private float _originalSpeed;
    
    public override void Enter()
    {
        // 1. Setup Initial State
        _originalSpeed = context.EntityMover.moveSpeed;
        context.EntityMover.SetMoveDirection(Vector2.zero);
        
        _windUpTimer = _windUpDuration;
        _chargeTimer = _chargeDuration;
        
        // 2. Lock in the direction at the moment of the wind-up
        if (context.currentTarget != null)
        {
            _chargeDirection = (context.currentTarget.position - context.transform.position).normalized;
            context.EntityAnimator.FaceDirection(_chargeDirection);
        }
    }

    public override void Update()
    {
        // 1. Wind Up
        if (_windUpTimer > 0)
        {
            _windUpTimer -= Time.deltaTime;
            context.EntityMover.SetMoveDirection(Vector2.zero);
            return; // Don't proceed to charge yet, still winding up
        }

        if (_chargeTimer > 0)
        {
            _chargeTimer -= Time.deltaTime;
            
            // Temporarily increase speed and move
            context.EntityMover.moveSpeed = _originalSpeed * _chargeSpeedMultiplier;
            context.EntityMover.SetMoveDirection(_chargeDirection);
        }
        else // Action finished
        {
            stateMachine.ChangeState(context.IdleState);
        }
    }

    public override void PhysicsUpdate() { }
    public override void HandleInput() { }

    public override void Exit()
    {
        context.EntityMover.moveSpeed = _originalSpeed;
        context.EntityMover.SetMoveDirection(Vector2.zero);
        context.ResetActionCooldown();
    }
    
}
