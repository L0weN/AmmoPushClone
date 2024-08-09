using Mert.EventBus;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIAmmoButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private AmmoData ammoData;

    [SerializeField] private Button ammoButton;
    [SerializeField] private Image ammoIcon;
    [SerializeField] private TextMeshProUGUI ammoText;

    private bool isHolding = false;
    private float delay = 0.1f;

    EventBinding<GameStateChanged> gameStateChangedEventBinding;

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
                ResetAmmo();
                break;
            case GameState.PREBOSS:
                CheckAmmoCount();
                SetAmmoText();
                break;
            case GameState.WON:
            case GameState.LOST:
                ammoButton.interactable = false;
                break;
        }
    }

    public void InitializeAmmoButton(AmmoData ammoData)
    {
        this.ammoData = ammoData;
        SetAmmoImage();
        SetAmmoText();
    }

    private void ResetAmmo()
    {
        ammoData.count = 0;
        SetAmmoText();
    }

    private void SetAmmoImage()
    {
        ammoIcon.sprite = ammoData.icon;
        ammoIcon.SetNativeSize();
    }

    private void SetAmmoText() => ammoText.text = "x " + ammoData.count.ToString();

    public void OnPointerDown(PointerEventData eventData)
    {
        isHolding = true;
        StartCoroutine(HoldRoutine());
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isHolding = false;
        StopCoroutine(HoldRoutine());
    }

    private IEnumerator HoldRoutine()
    {
        while (isHolding && ammoData.count > 0)
        {
            ammoData.count--;
            CheckAmmoCount();
            SetAmmoText();
            EventBus<AmmoFired>.Raise(new AmmoFired { AmmoData = ammoData });
            yield return new WaitForSeconds(delay);
        }
    }

    private void CheckAmmoCount() => ammoButton.interactable = ammoData.count > 0;
}
