using Mert.EventBus;

/// <summary>
/// This class represents the state of the game when the player is collecting items.
/// </summary>
public class CollectState : GameStateBase
{
    public CollectState(GameStateManager gameStateManager) : base(gameStateManager) { }

    public override void OnEnter()
    {
        gameStateManager.isTouched = false;
        EventBus<GameStateChanged>.Raise(new GameStateChanged { State = GameState.COLLECT });
    }
}