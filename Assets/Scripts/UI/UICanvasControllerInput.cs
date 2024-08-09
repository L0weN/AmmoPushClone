using Mert.EventBus;
using UnityEngine;

public class UICanvasControllerInput : MonoBehaviour
{
    [Header("Output")]
    [SerializeField] private PlayerInputs playerInputs;
    [SerializeField] private GameObject virtualJoystick;

    EventBinding<GameStateChanged> gameStateChangedEventBinding;

    public void VirtualMoveInput(Vector2 virtualMoveDirection)
    {
        playerInputs.MoveInput(virtualMoveDirection);
    }

    public void VirtualLookInput(Vector2 virtualLookDirection)
    {
        playerInputs.LookInput(virtualLookDirection);
    }

    private void OnEnable()
    {
        gameStateChangedEventBinding = new EventBinding<GameStateChanged>(HandleGameState);
        EventBus<GameStateChanged>.Register(gameStateChangedEventBinding);
    }

    private void OnDisable()
    {
        EventBus<GameStateChanged>.Unregister(gameStateChangedEventBinding);
    }

    private void HandleGameState(GameStateChanged gameStateChanged)
    {
        switch (gameStateChanged.State)
        {
            case GameState.COLLECT:
                virtualJoystick.SetActive(true);
                break;
            case GameState.POSTCOLLECT:
                virtualJoystick.SetActive(false);
                VirtualMoveInput(Vector2.zero);
                break;
        }
    }
}
