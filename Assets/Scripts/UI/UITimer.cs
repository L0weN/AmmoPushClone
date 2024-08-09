using DG.Tweening;
using Mert.EventBus;
using System.Collections;
using TMPro;
using UnityEngine;

public class UITimer : MonoBehaviour
{
    private RectTransform rectTransform;
    private float panelTransformChange = 200f;

    [SerializeField] private TextMeshProUGUI timerText;

    private float timerMultiplier = 0f;
    private const float minDefaultTimer = 30f;

    EventBinding<GameStateChanged> gameStateChangedEventBinding;
    EventBinding<PowerUpPurchased> powerUpPurchasedEventBinding;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void Start()
    {
        timerMultiplier = GameResources.Instance.GetPowerUpLevel("Time");
        InitializeTimer(minDefaultTimer + timerMultiplier);
    }

    private void OnEnable()
    {
        gameStateChangedEventBinding = new EventBinding<GameStateChanged>(HandleGameState);
        EventBus<GameStateChanged>.Register(gameStateChangedEventBinding);

        powerUpPurchasedEventBinding = new EventBinding<PowerUpPurchased>(HandlePowerUpPurchased);
        EventBus<PowerUpPurchased>.Register(powerUpPurchasedEventBinding);
    }

    private void OnDisable()
    {
        StopAllCoroutines();
        EventBus<GameStateChanged>.Unregister(gameStateChangedEventBinding);
        EventBus<PowerUpPurchased>.Unregister(powerUpPurchasedEventBinding);
    }

    private void HandleGameState(GameStateChanged gameStateChanged)
    {
        switch (gameStateChanged.State)
        {
            case GameState.COLLECT:
                rectTransform.DOAnchorPosY(rectTransform.anchoredPosition.y - panelTransformChange, Settings.DEFAULT_UI_ANIMATION_DURATION).SetEase(Ease.InOutQuad);
                StartCoroutine(StartTimer((minDefaultTimer + timerMultiplier), false));
                break;
            case GameState.POSTCOLLECT:
                StartCoroutine(StartTimer(3, true));
                break;
            case GameState.PREBOSS:
                rectTransform.DOAnchorPosY(rectTransform.anchoredPosition.y + panelTransformChange, Settings.DEFAULT_UI_ANIMATION_DURATION).SetEase(Ease.InOutQuad);
                break;
        }
    }

    private void HandlePowerUpPurchased(PowerUpPurchased powerUpPurchased)
    {
        timerMultiplier = GameResources.Instance.GetPowerUpLevel("Time");
        InitializeTimer(minDefaultTimer + timerMultiplier);
    }

    public void InitializeTimer(float timer)
    {
        timerText.SetText(timer.ToString("00"));
    }

    private IEnumerator StartTimer(float timer, bool isAlreadyCollected)
    {
        float time = timer;

        while (time >= 0)
        {
            time -= Time.deltaTime;
            timerText.SetText(time.ToString("00"));
            yield return null;
        }

        if (isAlreadyCollected)
            PrepareBoss();
        else
            EndCollect();
    }

    private void EndCollect() => GameStateManager.Instance.isCollected = true;

    private void PrepareBoss() => GameStateManager.Instance.isTimeEnd = true;
}
