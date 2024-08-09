using Mert.FSM;
using UnityEngine;
#if ENABLE_INPUT_SYSTEM 
using UnityEngine.InputSystem;
#endif

[RequireComponent(typeof(CharacterController))]
#if ENABLE_INPUT_SYSTEM
[RequireComponent(typeof(PlayerInput))]
#endif
public class PlayerController : MonoBehaviour
{
#if ENABLE_INPUT_SYSTEM
    private PlayerInput playerInput;
#endif
    private Animator animator;
    private CharacterController characterController;
    private PlayerInputs playerInputs;
    private GameObject mainCamera;
    public GameObject cinemachineCameraTarget;

    [Header("State Machine")]
    protected StateMachine stateMachine;

    [Tooltip("States")]
    protected PlayerIdleState idleState;
    protected PlayerMoveState moveState;

    private void Awake()
    {
#if ENABLE_INPUT_SYSTEM
        playerInput = GetComponent<PlayerInput>();
#endif
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        playerInputs = GetComponent<PlayerInputs>();

        mainCamera = Camera.main.gameObject;
    }

    private void Start()
    {
        stateMachine = new StateMachine();

        idleState = new PlayerIdleState(this, animator, characterController, playerInputs, playerInput, mainCamera, cinemachineCameraTarget);
        moveState = new PlayerMoveState(this, animator, characterController, playerInputs, playerInput, mainCamera, cinemachineCameraTarget);

        At(idleState, moveState, new FuncPredicate(() => playerInputs.move != Vector2.zero));
        At(moveState, idleState, new FuncPredicate(() => playerInputs.move == Vector2.zero));

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