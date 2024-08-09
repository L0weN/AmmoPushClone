using UnityEngine;

/// <summary>
/// This class is responsible for managing a power up data.
/// </summary>
[CreateAssetMenu(fileName = "PowerUp_", menuName = "Scriptable Objects/Power Up")]
public class PowerUpData : ScriptableObject
{
    public string powerUpName; // Name of the power up.
    public int powerUpLevel; // Level of the power up.
    public Sprite powerUpSprite; // Sprite of the power up.
    public Sprite powerUpPressedSprite; // Sprite of the power up when pressed.
    public Sprite powerUpDisabledSprite; // Sprite of the power up when disabled.
}
