/// <summary>
/// This interface is used to define the methods that a class must implement in order to be considered damageable.
/// </summary>
public interface IDamageable
{
    public int CurrentHealth { get; }
    public int MaxHealth { get; }
    public void TakeDamage(int damage);
}
