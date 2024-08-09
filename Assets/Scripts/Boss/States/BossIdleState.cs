using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// This class is responsible for the idle state of the boss character.
/// </summary>
public class BossIdleState : BossBaseState
{
    public BossIdleState(BossController bossController, Animator animator, NavMeshAgent agent, Health health) : base(bossController, animator, agent, health) { }

    public override void OnEnter()
    {
        animator.SetBool(Settings.idleAnimatorHash, true);
    }

    public override void OnExit()
    {
        animator.SetBool(Settings.idleAnimatorHash, false);
    }
}
