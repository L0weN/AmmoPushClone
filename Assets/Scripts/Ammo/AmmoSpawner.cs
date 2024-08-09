using System.Collections.Generic;
using UnityEngine;

public class AmmoSpawner : MonoBehaviour
{
    public Transform[] spawnTransforms;

    private void Start()
    {
        SpawnObjects();
    }

    void SpawnObjects()
    {
        List<AmmoData> ammoList = GameResources.Instance.GetAmmoList();
        AmmoData ammoData;

        do
        {
            ammoData = ammoList[Random.Range(0, ammoList.Count)];
        }
        while (ammoData.minPlayerLevelToSpawn > GameResources.Instance.GetPlayerLevel());

        foreach (Transform spawnTransform in spawnTransforms)
        {
            Instantiate(ammoData.prefab, spawnTransform.position, Quaternion.identity, transform);
        }
    }
}
