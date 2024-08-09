using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// This class is responsible for the attack state of the boss character.
/// </summary>
public class BossAttackState : BossBaseState
{
    public BossAttackState(BossController bossController, Animator animator, NavMeshAgent agent, Health health) : base(bossController, animator, agent, health) { }

    public override void OnEnter()
    {
        animator.SetBool(Settings.attackAnimatorHash, true);
        GameStateManager.Instance.isPlayerDead = true;
    }

    public override void OnExit()
    {
        animator.SetBool(Settings.attackAnimatorHash, false);
    }
}
