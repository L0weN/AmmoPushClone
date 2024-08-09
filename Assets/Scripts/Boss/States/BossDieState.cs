using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// This class is responsible for the die state of the boss character.
/// </summary>
public class BossDieState : BossBaseState
{
    public BossDieState(BossController bossController, Animator animator, NavMeshAgent agent, Health health) : base(bossController, animator, agent, health) { }

    public override void OnEnter()
    {
        GameStateManager.Instance.isBossDead = true;
        animator.SetBool(Settings.dieAnimatorHash, true);
    }

    public override void OnExit()
    {
        animator.SetBool(Settings.dieAnimatorHash, false);
    }
}
