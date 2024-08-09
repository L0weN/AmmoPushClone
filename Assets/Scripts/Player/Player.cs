using Mert.SaveLoadSystem;
using UnityEngine;

/// <summary>
/// This class is responsible for player data.
/// </summary>
public class Player : MonoBehaviour, IBind<PlayerData>
{
    public SerializableGuid Id { get; set; } = SerializableGuid.NewGuid();
    private PlayerData data;

    private int level;
    private float coin;

    public void Bind(PlayerData data)
    {
        this.data = data;
        this.data.Id = Id;
        level = data.playerLevel;
        coin = data.playerCoin;
    }
}
