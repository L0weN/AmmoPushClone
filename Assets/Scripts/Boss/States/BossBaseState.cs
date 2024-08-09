using Mert.FSM;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// This class is responsible for the base state of the boss character.
/// </summary>
public class BossBaseState : IState
{
    protected BossController bossController; // The boss controller.
    protected Animator animator; // The animator component of the boss.
    protected NavMeshAgent agent; // The NavMeshAgent component of the boss.
    protected Health health; // The health component of the boss.

    public BossBaseState(BossController bossController, Animator animator, NavMeshAgent agent, Health health)
    {

        this.bossController = bossController;
        this.animator = animator;
        this.agent = agent;
        this.health = health;
    }

    public virtual void OnEnter() { }
    public virtual void OnExit() { }
    public virtual void Update() { }
    public virtual void FixedUpdate() { }
    public virtual void LateUpdate() { }

    /// <summary>
    /// This method is responsible for moving the boss character.
    /// </summary>
    /// <param name="target"></param>
    public virtual void Move(GameObject target)
    {
        agent.SetDestination(target.transform.position);
    }
}
