using DG.Tweening;
using Mert.EventBus;
using UnityEngine;

public class UIAmmoPanel : MonoBehaviour
{
    private RectTransform rectTransform;

    private float panelTransformChange = 1100f;

    EventBinding<GameStateChanged> gameStateChangedEventBinding;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void Start()
    {
        SetUpAmmoPanel();
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
            case GameState.PREBOSS:
                rectTransform.DOAnchorPosX(rectTransform.anchoredPosition.x - panelTransformChange, Settings.DEFAULT_UI_ANIMATION_DURATION).SetEase(Ease.InOutQuad);
                break;
            case GameState.WON:
            case GameState.LOST:
                rectTransform.DOAnchorPosX(rectTransform.anchoredPosition.x + panelTransformChange, Settings.DEFAULT_UI_ANIMATION_DURATION).SetEase(Ease.InOutQuad);
                break;
        }
    }

    private void SetUpAmmoPanel()
    {
        foreach (AmmoData ammo in GameResources.Instance.GetAmmoList())
        {
            GameObject ammoButton = Instantiate(GameResources.Instance.ammoPrefab, transform);
            ammoButton.GetComponent<UIAmmoButton>().InitializeAmmoButton(ammo);
        }
    }
}
