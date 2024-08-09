using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// This class is responsible for the hit state of the boss character.
/// </summary>
public class BossHitState : BossBaseState
{
    public BossHitState(BossController bossController, Animator animator, NavMeshAgent agent, Health health) : base(bossController, animator, agent, health) { }

    public override void OnEnter()
    {
        animator.SetBool(Settings.hitAnimatorHash, true);
        agent.speed = 2f;
        health.isTakingDamage = false;
    }

    public override void OnExit()
    {
        agent.speed = 3.5f;
        animator.SetBool(Settings.hitAnimatorHash, false);
    }
}
