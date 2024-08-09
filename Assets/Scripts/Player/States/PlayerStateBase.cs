using Mert.FSM;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// This class is responsible for player state base.
/// </summary>
public class PlayerStateBase : IState
{
    protected PlayerController playerController;
    protected Animator animator;
    protected CharacterController characterController;
    protected PlayerInputs playerInputs;
    protected PlayerInput playerInput;
    protected GameObject mainCamera;

    protected float MoveSpeed = 25f;
    protected float RotationSmoothTime = 0.12f;
    protected float SpeedChangeRate = 100f;
    protected float Gravity = 0.1f;

    protected GameObject cinemachineCameraTarget;
    protected float TopClamp = 70.0f;
    protected float BottomClamp = -30.0f;
    protected float CameraAngleOverride = 0.0f;
    protected bool LockCameraPosition = true;

    public bool Grounded = true;
    public float GroundedOffset = -0.14f;
    public float GroundedRadius = 0.28f;
    public LayerMask GroundLayers;

    // cinemachine
    protected float _cinemachineTargetYaw;
    protected float _cinemachineTargetPitch;

    // player
    protected float _speed;
    protected float _animationBlend;
    protected float _targetRotation = 0.0f;
    protected float _rotationVelocity;
    protected float _verticalVelocity;

    protected const float _threshold = 0.01f;

    protected bool IsCurrentDeviceMouse
    {
        get
        {
#if ENABLE_INPUT_SYSTEM
            return playerInput.currentControlScheme == "KeyboardMouse";
#else
				return false;
#endif
        }
    }

    protected PlayerStateBase(PlayerController playerController, Animator animator, CharacterController characterController, PlayerInputs playerInputs, PlayerInput playerInput, GameObject mainCamera , GameObject cinemachineCameraTarget)
    {
        this.playerController = playerController;
        this.animator = animator;
        this.characterController = characterController;
        this.playerInputs = playerInputs;
        this.mainCamera = mainCamera;
        this.playerInput = playerInput;
        this.cinemachineCameraTarget = cinemachineCameraTarget;
    }

    public virtual void OnEnter() { }

    public virtual void OnExit() { }

    public virtual void Update() { }

    public virtual void FixedUpdate() { }

    public virtual void LateUpdate() { }

    /// <summary>
    /// This method is responsible for moving the player.
    /// </summary>
    protected virtual void Move()
    {
        float targetSpeed =  MoveSpeed; // by default move speed is 10

        if (playerInputs.move == Vector2.zero) targetSpeed = 0.0f; // if there is no input, set speed to 0

        float currentHorizontalSpeed = new Vector3(characterController.velocity.x, 0.0f, characterController.velocity.z).magnitude; // get current horizontal speed

        float speedOffset = 0.1f; // speed offset
        float inputMagnitude = playerInputs.analogMovement ? playerInputs.move.magnitude : 1f; // get input magnitude

        if (currentHorizontalSpeed < targetSpeed - speedOffset ||
            currentHorizontalSpeed > targetSpeed + speedOffset)
        {
            _speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude,
                Time.deltaTime * SpeedChangeRate); // lerp speed

            _speed = Mathf.Round(_speed * 1000f) / 1000f; // speed rounding
        }
        else
        {
            _speed = targetSpeed; // set speed to target speed
        }

        _animationBlend = Mathf.Lerp(_animationBlend, targetSpeed, Time.deltaTime * SpeedChangeRate); // lerp animation blend
        if (_animationBlend < 0.01f) _animationBlend = 0f; // if animation blend is less than 0.01, set it to 0

        Vector3 inputDirection = new Vector3(playerInputs.move.x, 0.0f, playerInputs.move.y).normalized; // get input direction

        if (playerInputs.move != Vector2.zero)
        {
            _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg +
                              mainCamera.transform.eulerAngles.y; // get target rotation
            float rotation = Mathf.SmoothDampAngle(characterController.gameObject.transform.eulerAngles.y, _targetRotation, ref _rotationVelocity,
                RotationSmoothTime); // smooth damp rotation

            characterController.gameObject.transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f); // set rotation
        }

        Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward; // get target direction

        characterController.Move(targetDirection.normalized * (_speed * Time.deltaTime) +
                         new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime); // move character controller
    }

    private void GroundedCheck()
    {
        Vector3 spherePosition = new Vector3(playerController.transform.position.x, playerController.transform.position.y - GroundedOffset,
            playerController.transform.position.z);
        Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers,
            QueryTriggerInteraction.Ignore);
    }

    protected virtual void ApplyGravity()
    {
        GroundedCheck();

        if (Grounded && _verticalVelocity < 0.0f)
        {
            _verticalVelocity = 0f;
        }
        else
        {
            _verticalVelocity -= Gravity * Time.deltaTime;
        }

        characterController.Move(new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);
    }

    /// <summary>
    /// This method is responsible for camera rotation.
    /// </summary>
    protected virtual void CameraRotation()
    {
        // if there is an input and camera position is not fixed
        if (playerInputs.look.sqrMagnitude >= _threshold && !LockCameraPosition)
        {
            //Don't multiply mouse input by Time.deltaTime;
            float deltaTimeMultiplier = IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;

            _cinemachineTargetYaw += playerInputs.look.x * deltaTimeMultiplier;
            _cinemachineTargetPitch += playerInputs.look.y * deltaTimeMultiplier;
        }

        // clamp our rotations so our values are limited 360 degrees
        _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
        _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

        // Cinemachine will follow this target
        cinemachineCameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch + CameraAngleOverride,
            _cinemachineTargetYaw, 0.0f);
    }

    /// <summary>
    /// This method is responsible for clamping angle.
    /// </summary>
    /// <param name="lfAngle"></param>
    /// <param name="lfMin"></param>
    /// <param name="lfMax"></param>
    /// <returns></returns>
    private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }
}
