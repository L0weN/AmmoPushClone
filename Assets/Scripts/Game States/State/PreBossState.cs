using Mert.EventBus;

/// <summary>
/// This class is responsible for the PreBoss state of the game.
/// </summary>
public class PreBossState : GameStateBase
{
    public PreBossState(GameStateManager gameStateManager) : base(gameStateManager) { }

    public override void OnEnter()
    {
        gameStateManager.isTimeEnd = false;
        EventBus<GameStateChanged>.Raise(new GameStateChanged() { State = GameState.PREBOSS });
    }
}