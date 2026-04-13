
public abstract class State
{
    protected StateMachine stateMachine;
    
    public State(StateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }

    // Called upon entering state
    public virtual void Enter(){}
    public virtual void HandleInput(){}

    public virtual void Update(){}

    public virtual void PhysicsUpdate(){}

    
    // Called upon exiting state
    public virtual void Exit(){}
}

// Generic State
// Other classes which inherit from State<T> must define T, where T must inherit from a BaseEntityController
public abstract class State<T> : State where T : BaseEntityController
{
    // Specialized Reference holds the controller (PlayerController / EntityController)
    protected T context;

    // Constructor: Pass the specific controller (context) and the stateMachine it belongs to
    public State(T context, StateMachine stateMachine) : base(stateMachine)
    {
        this.context = context; // Save the reference to use inside Update/Enter methods.
    }
}