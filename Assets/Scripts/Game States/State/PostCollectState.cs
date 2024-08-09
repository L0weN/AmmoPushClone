using Mert.EventBus;

/// <summary>
/// This class represents the post collect state of the game.
/// </summary>
public class PostCollectState : GameStateBase
{
    public PostCollectState(GameStateManager gameStateManager) : base(gameStateManager) { }

    public override void OnEnter()
    {
        gameStateManager.isCollected = false;
        EventBus<GameStateChanged>.Raise(new GameStateChanged() { State = GameState.POSTCOLLECT });
    }
}