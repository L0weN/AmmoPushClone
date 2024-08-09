using Mert.EventBus;

/// <summary>
/// This class is responsible for the PreCollect state of the game.
/// </summary>
public class PreCollectState : GameStateBase
{
    public PreCollectState(GameStateManager gameStateManager) : base(gameStateManager) { }

    public override void OnEnter()
    {
        gameStateManager.isStartGame = false;
        EventBus<GameStateChanged>.Raise(new GameStateChanged { State = GameState.PRECOLLECT });
    }
}