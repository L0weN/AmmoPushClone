using System;
using UnityEngine;

/// <summary>
/// Health class is used to manage the health of a game object.
/// </summary>
public class Health : MonoBehaviour, IDamageable
{
    public event Action<HealthChangedEventArgs> HealthChanged; // Event that is invoked when the health of the game object changes.
    public int CurrentHealth { get => currentHealth; }
    public int MaxHealth { get => maxHealth; }

    public bool isTakingDamage = false;

    private int currentHealth;
    private int maxHealth;

    private void Awake()
    {
        maxHealth = GameResources.Instance.GetPlayerLevel() * 100;
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth = Mathf.Clamp(currentHealth - damage, 0, maxHealth);
        HealthChanged?.Invoke(new HealthChangedEventArgs { CurrentHealth = currentHealth });
        if (currentHealth == 0)
        {
            GameStateManager.Instance.isBossDead = true;
        }
    }
}

/// <summary>
/// This class is used to pass the current health of a game object to the event handler.
/// </summary>
public class HealthChangedEventArgs : EventArgs
{
    public int CurrentHealth;
}

