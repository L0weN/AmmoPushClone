using UnityEngine;

/// <summary>
/// This class is used to store data for ammo.
/// </summary>
[CreateAssetMenu(fileName = "Ammo_", menuName = "Scriptable Objects/Ammo")]
public class AmmoData : ScriptableObject
{
    public string ammoName; // Name of the ammo
    public GameObject prefab; // Prefab of the ammo
    public Sprite icon; // Icon of the ammo
    public int damage = 1; // Damage of the ammo
    public int count = 0; // Count of the ammo
    public int minPlayerLevelToSpawn = 1; // Minimum player level to spawn the ammo
    public int poolSize = 10; // Pool size of the ammo
    public Quaternion spawnRotation = Quaternion.identity;
}
