using DG.Tweening;
using Mert.EventBus;
using Mert.SaveLoadSystem;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Resources that are used in the game.
/// </summary>
public class GameResources : SingleBehaviour<GameResources>
{
    [Header("SAVE")]
    [Tooltip("Game data that will be saved.")]
    public GameData gameData;

    [Header("Power Up List")]
    [Tooltip("List of power ups that are used in the game.")]
    public List<PowerUpData> powerUpList;

    [Header("Ammo List")]
    [Tooltip("List of ammo that are used in the game.")]
    public List<AmmoData> ammoList;

    [Header("UI Prefab")]
    [Tooltip("PowerUpButton prefab that will be used in the game.")]
    public GameObject powerUpPrefab;
    [Tooltip("AmmoButton prefab that will be used in the game.")]
    public GameObject ammoPrefab;

    EventBinding<PlayerLevelChanged> playerLevelChangedEventBinding;
    EventBinding<CoinChanged> coinChangedEventBinding;
    EventBinding<PowerUpPurchased> powerUpPurchasedChangedEventBinding;

    private void Start()
    {
        LoadGameResourcesFromSave();
    }

    private void OnEnable()
    {
        playerLevelChangedEventBinding = new EventBinding<PlayerLevelChanged>(HandlePlayerLevel);
        EventBus<PlayerLevelChanged>.Register(playerLevelChangedEventBinding);

        coinChangedEventBinding = new EventBinding<CoinChanged>(HandleCoinChanged);
        EventBus<CoinChanged>.Register(coinChangedEventBinding);

        powerUpPurchasedChangedEventBinding = new EventBinding<PowerUpPurchased>(HandlePowerUpPurchased);
        EventBus<PowerUpPurchased>.Register(powerUpPurchasedChangedEventBinding);
    }

    private void OnDisable()
    {
        EventBus<PlayerLevelChanged>.Unregister(playerLevelChangedEventBinding);
        EventBus<CoinChanged>.Unregister(coinChangedEventBinding);
        EventBus<PowerUpPurchased>.Unregister(powerUpPurchasedChangedEventBinding);
    }

    private void OnApplicationQuit()
    {
        DOTween.KillAll();
        SaveGameResources();
    }

    private void LoadGameResourcesFromSave()
    {
        SaveManager.Instance.LoadGame(Settings.DEFAULT_GAME_NAME);
        gameData = SaveManager.Instance.GetGameData();
    }

    private void SaveGameResources()
    {
        SaveManager.Instance.SetGameData(gameData);
        SaveManager.Instance.SaveGame();
    }

    private void HandlePlayerLevel(PlayerLevelChanged playerLevelChanged) => gameData.playerData.playerLevel = playerLevelChanged.Level;
    private void HandleCoinChanged(CoinChanged coinChanged) => gameData.playerData.playerCoin = coinChanged.Coin;
    private void HandlePowerUpPurchased(PowerUpPurchased powerUpPurchased) => powerUpList.Find(powerUp => powerUp.powerUpName == powerUpPurchased.PowerUpData.powerUpName).powerUpLevel++;

    public int GetPlayerLevel() => gameData.playerData.playerLevel;
    public float GetPlayerCoin() => gameData.playerData.playerCoin;

    public List<PowerUpData> GetPowerUpList() => powerUpList;
    public List<AmmoData> GetAmmoList() => ammoList;

    public int GetPowerUpLevel(string powerUpName) => powerUpList.Find(powerUp => powerUp.powerUpName == powerUpName).powerUpLevel;
    public int GetAmmoCount(AmmoData ammo) => ammoList.Find(ammo => ammo.Equals(ammo)).count;
}
