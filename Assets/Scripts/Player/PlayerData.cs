using Mert.SaveLoadSystem;
using System;
using UnityEngine;

/// <summary>
/// This class is responsible for saving and loading player data.
/// </summary>
[Serializable]
public class PlayerData : ISaveable
{
    [SerializeField] public SerializableGuid Id { get; set; }
    public int playerLevel;
    public float playerCoin;
}
