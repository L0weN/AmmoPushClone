using UnityEngine;

/// <summary>
/// This class represents an ammunition object that can be fired at a target.
/// </summary>
public class Ammo : MonoBehaviour
{
    public AmmoData ammoData; // Data of the ammo

    private void OnCollisionEnter(Collision collision)
    {
        DetectCollisionWithBoss(collision);
    }

    private void DetectCollisionWithBoss(Collision collision)
    {
        if (collision.gameObject.CompareTag(Settings.BOSS_TAG))
        {
            if (collision.gameObject.TryGetComponent(out IDamageable damageable))
            {
                damageable.TakeDamage(ammoData.damage);
            }
            AmmoPoolManager.Instance.ReleaseAmmo(ammoData, gameObject);
        }
    }
}
