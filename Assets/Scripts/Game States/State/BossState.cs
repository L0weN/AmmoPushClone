using Mert.EventBus;

/// <summary>
/// This class represents the state of the game when the player is fighting the boss.
/// </summary>
public class BossState : GameStateBase
{
    public BossState(GameStateManager gameStateManager) : base(gameStateManager) { }

    public override void OnEnter()
    {
        gameStateManager.isTouched = false;
        EventBus<GameStateChanged>.Raise(new GameStateChanged() { State = GameState.BOSS });
    }
}