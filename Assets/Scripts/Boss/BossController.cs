using Mert.FSM;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// This class is responsible for controlling the boss character.
/// </summary>
/// 
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Health))]
public class BossController : MonoBehaviour
{
    public GameObject target; // The target that the boss will follow and attack.

    private NavMeshAgent agent; // The NavMeshAgent component of the boss.
    private Animator animator; // The Animator component of the boss.
    private Health health; // The Health component of the boss.

    [Header("State Machine")]
    private StateMachine stateMachine; // The state machine of the boss.

    [Tooltip("States")]
    private BossIdleState idleState; // The idle state of the boss.
    private BossMoveState moveState; // The move state of the boss.
    private BossAttackState attackState; // The attack state of the boss.
    private BossHitState hitState; // The hit state of the boss.
    private BossDieState dieState; // The die state of the boss.

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        health = GetComponent<Health>();
    }

    private void Start()
    {
        stateMachine = new StateMachine();

        idleState = new BossIdleState(this, animator, agent, health);
        moveState = new BossMoveState(this, animator, agent, health);
        attackState = new BossAttackState(this, animator, agent, health);
        hitState = new BossHitState(this, animator, agent, health);
        dieState = new BossDieState(this, animator, agent, health);

        At(idleState, moveState, new FuncPredicate(() => Input.touchCount > 0 || Input.GetMouseButtonDown(0))); // When the player clicks the left mouse button, the boss will move.
        At(moveState, attackState, new FuncPredicate(() => agent.remainingDistance < 2f)); // When agent reaches the target, it will attack.
        At(hitState, moveState, new FuncPredicate(() => agent.remainingDistance > 2f)); // Boss will move after taking damage.
        Any(hitState, new FuncPredicate(() => health.isTakingDamage)); // Boss will take damage.
        Any(dieState, new FuncPredicate(() => health.CurrentHealth <= 0)); // Boss will die when its health is less than or equal to 0.

        stateMachine.SetState(idleState);
    }

    private void Update()
    {
        stateMachine.Update();
    }

    private void FixedUpdate()
    {
        stateMachine.FixedUpdate();
    }

    private void LateUpdate()
    {
        stateMachine.LateUpdate();
    }

    protected void At(IState from, IState to, IPredicate condition) => stateMachine.AddTransition(from, to, condition);
    protected void Any(IState to, IPredicate condition) => stateMachine.AddAnyTransition(to, condition);
}
