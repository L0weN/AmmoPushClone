using Mert.FSM;

/// <summary>
/// This class is the base class for all game states.
/// </summary>
public class GameStateBase : IState
{
    protected GameStateManager gameStateManager;

    protected GameStateBase(GameStateManager gameStateManager)
    {
        this.gameStateManager = gameStateManager;
    }

    public virtual void OnEnter() { }

    public virtual void OnExit() { }

    public virtual void Update() { }

    public virtual void FixedUpdate() { }

    public virtual void LateUpdate() { }
}