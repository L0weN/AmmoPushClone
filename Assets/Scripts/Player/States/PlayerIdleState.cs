using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// This class is responsible for player idle state.
/// </summary>
public class PlayerIdleState : PlayerStateBase
{
    public PlayerIdleState(PlayerController playerController, Animator animator, CharacterController characterController, PlayerInputs playerInputs, PlayerInput playerInput, GameObject mainCamera, GameObject cinemachineCameraTarget) : base(playerController, animator, characterController, playerInputs, playerInput, mainCamera, cinemachineCameraTarget) { }

    public override void OnEnter()
    {
        animator.SetBool(Settings.idleAnimatorHash, true); // idle animation
    }

    public override void Update()
    {
        ApplyGravity();
    }

    public override void OnExit()
    {
        animator.SetBool(Settings.idleAnimatorHash, false); // idle animation
    }
}
