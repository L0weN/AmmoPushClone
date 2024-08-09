using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.EventSystems;
using Mert.EventBus;

public class UIStartButton : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private TextMeshProUGUI startText;

    EventBinding<GameStateChanged> gameStateChangedEventBinding;

    private void Start()
    {
        startText.transform.DOScale(new Vector3(1.2f, 1.2f, 1.2f), 1f).SetLoops(-1, LoopType.Yoyo);
        startText.transform.DORotate(new Vector3(0, 0, 10f), 0.5f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
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
            case GameState.PRECOLLECT:
            case GameState.PREBOSS:
                startText.gameObject.SetActive(true);
                break;
            case GameState.COLLECT:
            case GameState.BOSS:
                startText.transform.DOScale(Vector3.zero, Settings.DEFAULT_UI_ANIMATION_DURATION).SetEase(Ease.InOutSine).onComplete += () => startText.gameObject.SetActive(false);
                break;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        GameStateManager.Instance.isTouched = true;
    }
}
