using Mert.FSM;
using UnityEngine;

/// <summary>
/// This class is used to manage the game states.
/// </summary>
public class GameStateManager : SingleBehaviour<GameStateManager>
{
    protected StateMachine stateMachine; // State machine to manage the game states

    /// <summary>
    /// States of the game.
    /// </summary>
    protected PreCollectState preCollectState;
    protected CollectState collectState;
    protected PostCollectState postCollectState;
    protected PreBossState preBossState;
    protected BossState bossState;
    protected WonState gameWonState;
    protected LostState gameLostState;

    /// <summary>
    /// Booleans to check the game state.
    /// </summary>
    public bool isStartGame = false;
    public bool isTouched = false;
    public bool isCollected = false;
    public bool isTimeEnd = false;
    public bool isBossDead = false;
    public bool isPlayerDead = false;

    protected void Start()
    {
        stateMachine = new StateMachine();

        preCollectState = new PreCollectState(this);
        collectState = new CollectState(this);
        postCollectState = new PostCollectState(this);
        preBossState = new PreBossState(this);
        bossState = new BossState(this);
        gameWonState = new WonState(this);
        gameLostState = new LostState(this);

        At(preCollectState, collectState, new FuncPredicate(() => isTouched));
        At(collectState, postCollectState, new FuncPredicate(() => isCollected));
        At(postCollectState, preBossState, new FuncPredicate(() => isTimeEnd));
        At(preBossState, bossState, new FuncPredicate(() => isTouched));
        At(bossState, gameWonState, new FuncPredicate(() => isBossDead));
        At(bossState, gameLostState, new FuncPredicate(() => isPlayerDead));
        Any(preCollectState, new FuncPredicate(() => isStartGame));

        stateMachine.SetState(preCollectState);
    }

    private void Update()
    {
        stateMachine.Update();
    }

    private void FixedUpdate()
    {
        stateMachine.FixedUpdate();
    }

    protected void At(IState from, IState to, IPredicate condition) => stateMachine.AddTransition(from, to, condition);
    protected void Any(IState to, IPredicate condition) => stateMachine.AddAnyTransition(to, condition);
}

