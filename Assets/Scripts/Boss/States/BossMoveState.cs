using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// This class is responsible for the move state of the boss character.
/// </summary>
public class BossMoveState : BossBaseState
{
    public BossMoveState(BossController bossController, Animator animator, NavMeshAgent agent, Health health) : base(bossController, animator, agent, health) { }

    public override void OnEnter()
    {
        animator.SetBool(Settings.moveAnimatorHash, true);
        Move(bossController.target);
    }

    public override void OnExit()
    {
        animator.SetBool(Settings.moveAnimatorHash, false);
        agent.ResetPath();
    }
}
