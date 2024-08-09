using UnityEngine;

public class AmmoCollector : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(Settings.AMMO_TAG))
        {
            AmmoData ammoData = other.gameObject.GetComponent<Ammo>().ammoData;
            GameResources.Instance.ammoList.Find(ammo => ammo == ammoData).count++;
        }
    }
}
