using Mert.EventBus;
using TMPro;
using UnityEngine;

public class UIPlayerData : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI coinText;

    EventBinding<PlayerLevelChanged> playerLevelChangedEventBinding;
    EventBinding<CoinChanged> coinChangedEventBinding;

    private void Start()
    {
        SetLevelText(GameResources.Instance.GetPlayerLevel());
        SetCoinText(GameResources.Instance.GetPlayerCoin());
    }

    private void OnEnable()
    {
        playerLevelChangedEventBinding = new EventBinding<PlayerLevelChanged>(HandlePlayerLevel);
        EventBus<PlayerLevelChanged>.Register(playerLevelChangedEventBinding);

        coinChangedEventBinding = new EventBinding<CoinChanged>(HandleCoin);
        EventBus<CoinChanged>.Register(coinChangedEventBinding);
    }

    private void OnDisable()
    {
        EventBus<CoinChanged>.Unregister(coinChangedEventBinding);
        EventBus<PlayerLevelChanged>.Unregister(playerLevelChangedEventBinding);
    }

    private void HandlePlayerLevel(PlayerLevelChanged playerLevelChanged)
    {
        SetLevelText(playerLevelChanged.Level);
    }

    private void HandleCoin(CoinChanged coinChanged)
    {
        SetCoinText(coinChanged.Coin);
    }

    public void SetLevelText(int level)
    {
        levelText.text = "Lv  " + level;
    }

    public void SetCoinText(float coin)
    {
        coinText.text = coin.ToString("####,0" + " $");
    }
}
