using DG.Tweening;
using Mert.EventBus;
using TMPro;
using UnityEngine;

public class UIRewardPanel : MonoBehaviour
{
    private RectTransform rectTransform;

    private float panelTransformChange = 1920f;

    EventBinding<GameStateChanged> gameStateChangedEventBinding;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
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
            case GameState.WON:
            case GameState.LOST:
                rectTransform.DOMoveY(rectTransform.position.y - panelTransformChange, Settings.DEFAULT_UI_ANIMATION_DURATION).SetEase(Ease.OutBounce).onComplete += () => DOTween.Kill(rectTransform);
                break;
            case GameState.PRECOLLECT:
                rectTransform.position = new Vector3(rectTransform.position.x, rectTransform.position.y + panelTransformChange, rectTransform.position.z);
                break;
        }
    }
}
