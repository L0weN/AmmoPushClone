using UnityEngine;
using UnityEngine.UI;

public class UIHealthBar : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private Health health;

    private void Start()
    {
        InitializeHealthBar();
    }

    void InitializeHealthBar()
    {
        slider.maxValue = health.MaxHealth;
        slider.value = health.CurrentHealth;
    }

    private void OnEnable()
    {
        health.HealthChanged += OnHealthChanged;
    }

    private void OnDisable()
    {
        health.HealthChanged -= OnHealthChanged;
    }

    private void OnHealthChanged(HealthChangedEventArgs args)
    {
        slider.value = args.CurrentHealth;
    }
}
