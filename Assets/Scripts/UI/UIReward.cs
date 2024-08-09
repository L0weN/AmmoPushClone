using Mert.EventBus;
using TMPro;
using UnityEngine;

public class UIReward : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI rewardText;
    [SerializeField] private TextMeshProUGUI rewardAmountText;

    private float rewardAmount;
    private bool isWon;

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
            case GameState.WON:
                rewardAmount = GameResources.Instance.GetPlayerLevel() * Settings.DEFAULT_WON_COIN_MULTIPLIER;
                SetRewardText(rewardText, "Next Mission!");
                isWon = true;
                break;
            case GameState.LOST:
                rewardAmount = GameResources.Instance.GetPlayerLevel() * Settings.DEFAULT_LOST_COIN_MULTIPLIER;
                SetRewardText(rewardText, "Try Again!");
                isWon = false;
                break;
        }

        SetRewardText(rewardAmountText, rewardAmount.ToString());
    }

    private void SetRewardText(TextMeshProUGUI text, string reward)
    {
        text.text = reward;
    }

    public void ClaimReward()
    {
        if (isWon)
        {
            EventBus<PlayerLevelChanged>.Raise(new PlayerLevelChanged { Level = GameResources.Instance.GetPlayerLevel() + 1 });
        }

        EventBus<CoinChanged>.Raise(new CoinChanged { Coin = GameResources.Instance.GetPlayerCoin() + rewardAmount });

        GameStateManager.Instance.isStartGame = true;
    }
}
