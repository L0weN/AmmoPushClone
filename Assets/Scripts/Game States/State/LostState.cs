using Mert.EventBus;

/// <summary>
/// This class represents the lost state of the game.
/// </summary>
public class LostState : GameStateBase
{
    public LostState(GameStateManager gameStateManager) : base(gameStateManager) { }

    public override void OnEnter()
    {
        gameStateManager.isPlayerDead = false;
        EventBus<GameStateChanged>.Raise(new GameStateChanged() { State = GameState.LOST });
    }
}