using Mert.EventBus;
using UnityEngine;
using DG.Tweening;

public class Cannon : MonoBehaviour
{
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject boss;

    EventBinding<AmmoFired> ammoFiredEventBinding;

    private void OnEnable()
    {
        ammoFiredEventBinding = new EventBinding<AmmoFired>(HandleAmmoFired);
        EventBus<AmmoFired>.Register(ammoFiredEventBinding);
    }

    private void OnDisable()
    {
        EventBus<AmmoFired>.Unregister(ammoFiredEventBinding);
    }

    private void HandleAmmoFired(AmmoFired ammoFired)
    {
        GameObject ammo = AmmoPoolManager.Instance.GetAmmo(ammoFired.AmmoData);
        ammo.transform.localPosition = firePoint.position;
        ammo.transform.localRotation = ammoFired.AmmoData.spawnRotation;
        ammo.transform.DOLocalMove(boss.transform.position, 0.1f);
    }
}
