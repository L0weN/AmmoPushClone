using Mert.EventBus;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIPowerUpButton : MonoBehaviour
{
    private PowerUpData powerUpData;

    [SerializeField] private Image powerUpImage;
    [SerializeField] private Button powerUpButton;
    [SerializeField] private TextMeshProUGUI powerUpLevelText;
    [SerializeField] private TextMeshProUGUI powerUpCostText;

    EventBinding<CoinChanged> coinChangedEventBinding;

    private void Start()
    {
        CheckCoinForCost(GameResources.Instance.GetPlayerCoin());
    }

    private void OnEnable()
    {
        coinChangedEventBinding = new EventBinding<CoinChanged>(HandleCoinChanged);
        EventBus<CoinChanged>.Register(coinChangedEventBinding);
    }

    private void OnDisable()
    {
        EventBus<CoinChanged>.Unregister(coinChangedEventBinding);
    }

    private void HandleCoinChanged(CoinChanged coinChanged)
    {
        CheckCoinForCost(coinChanged.Coin);
    }

    public void InitializePowerUpButton(PowerUpData powerUpData)
    {
        this.powerUpData = powerUpData;

        SetPowerUpImage();
        SetPowerUpText();
        
        powerUpButton.onClick.AddListener(() => UpdatePowerUpLevel());
    }

    public void UpdatePowerUpLevel()
    {
        EventBus<PowerUpPurchased>.Raise(new PowerUpPurchased { PowerUpData = powerUpData });
        EventBus<CoinChanged>.Raise(new CoinChanged { Coin = GameResources.Instance.GetPlayerCoin() - CalculatePowerUpCost() });

        SetText(powerUpLevelText, powerUpData.powerUpLevel.ToString() + " Lv");
        SetText(powerUpCostText, CalculatePowerUpCost().ToString());
    }

    private void SetPowerUpImage()
    {
        powerUpImage.sprite = powerUpData.powerUpSprite;
        powerUpImage.SetNativeSize();

        powerUpButton.spriteState = new SpriteState
        {
            pressedSprite = powerUpData.powerUpPressedSprite,
            disabledSprite = powerUpData.powerUpDisabledSprite
        };
    }

    private void SetPowerUpText()
    {
        SetText(powerUpLevelText, powerUpData.powerUpLevel.ToString() + " Lv");
        SetText(powerUpCostText, CalculatePowerUpCost().ToString());
    }

    private void CheckCoinForCost(float coin) => powerUpButton.interactable = coin >= CalculatePowerUpCost();

    private void SetText(TextMeshProUGUI text, string value) => text.SetText(value);

    private float CalculatePowerUpCost() => powerUpData.powerUpLevel * Settings.DEFAULT_POWERUP_COST;
}
