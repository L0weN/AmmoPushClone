using DG.Tweening;
using Mert.EventBus;
using UnityEngine;

public class UIPowerUpPanel : MonoBehaviour
{
    private RectTransform rectTransform;

    private float panelTransformChange = 530f;

    EventBinding<GameStateChanged> gameStateEventBinding;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void Start()
    {
        SetPowerUpPanel();
    }

    private void OnEnable()
    {
        gameStateEventBinding = new EventBinding<GameStateChanged>(HandleGameStateChange);
        EventBus<GameStateChanged>.Register(gameStateEventBinding);
    }

    private void OnDisable()
    {
        EventBus<GameStateChanged>.Unregister(gameStateEventBinding);
    }

    private void HandleGameStateChange(GameStateChanged gameStateChanged)
    {
        switch (gameStateChanged.State)
        {
            case GameState.PRECOLLECT:
                rectTransform.DOAnchorPosY(rectTransform.anchoredPosition.y + panelTransformChange, Settings.DEFAULT_UI_ANIMATION_DURATION).SetEase(Ease.InOutQuad);
                break;
            case GameState.COLLECT:
                rectTransform.DOAnchorPosY(rectTransform.anchoredPosition.y - panelTransformChange, Settings.DEFAULT_UI_ANIMATION_DURATION).SetEase(Ease.InOutQuad);
                break;
        }

    }

    private void SetPowerUpPanel()
    {
        foreach (PowerUpData powerUp in GameResources.Instance.GetPowerUpList())
        {
            GameObject powerUpButton = Instantiate(GameResources.Instance.powerUpPrefab, transform);
            powerUpButton.GetComponent<UIPowerUpButton>().InitializePowerUpButton(powerUp);
        }
    }
}
