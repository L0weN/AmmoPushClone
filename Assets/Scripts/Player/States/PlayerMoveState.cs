using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// This class is responsible for player move state.
/// </summary>
public class PlayerMoveState : PlayerStateBase
{
    public PlayerMoveState(PlayerController playerController, Animator animator, CharacterController characterController, PlayerInputs playerInputs, PlayerInput playerInput, GameObject mainCamera, GameObject cinemachineCameraTarget) : base(playerController, animator, characterController, playerInputs, playerInput, mainCamera, cinemachineCameraTarget) { }

    public override void OnEnter()
    {
        animator.SetBool(Settings.moveAnimatorHash, true); // move animation
    }

    public override void Update()
    {
        Move(); // move
        ApplyGravity(); // gravity
    }

    public override void LateUpdate()
    {
        CameraRotation(); // camera rotation
    }

    public override void OnExit()
    {
        animator.SetBool(Settings.moveAnimatorHash, false); // move animation
    }
}
