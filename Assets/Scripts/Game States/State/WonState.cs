using Mert.EventBus;

/// <summary>
/// This class is responsible for the game state when the player wins the game.
/// </summary>
public class WonState : GameStateBase
{
    public WonState(GameStateManager gameStateManager) : base(gameStateManager) { }

    public override void OnEnter()
    {
        gameStateManager.isBossDead = false;
        EventBus<GameStateChanged>.Raise(new GameStateChanged() { State = GameState.WON });
    }
}