using Mert.EventBus;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is responsible for managing a pool of ammo objects.
/// </summary>
public class AmmoPoolManager : MonoBehaviour
{
    private static AmmoPoolManager instance;

    public static AmmoPoolManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<AmmoPoolManager>();
            }

            return instance;
        }
    }

    private List<AmmoData> ammoDatas; // List of ammo data.
    private Dictionary<AmmoData, ObjectPool> pools; // Dictionary of ammo data and object pools.

    EventBinding<GameStateChanged> gameStateChangedEventBinding;

    private void Start()
    {
        ammoDatas = GameResources.Instance.ammoList;
        pools = new Dictionary<AmmoData, ObjectPool>();

        foreach (var ammoData in ammoDatas)
        {
            ObjectPool pool = new GameObject($"Pool_{ammoData.name}").AddComponent<ObjectPool>();
            pool.transform.SetParent(transform);
            pool.Initialize(ammoData.prefab, ammoData.poolSize);
            pools[ammoData] = pool;
        }
    }

    private void OnEnable()
    {
        gameStateChangedEventBinding = new EventBinding<GameStateChanged>(HandleGameState);
        EventBus<GameStateChanged>.Register(gameStateChangedEventBinding);
    }

    private void OnDisable()
    {
        EventBus<GameStateChanged>.Unregister(gameStateChangedEventBinding);
    }

    private void HandleGameState(GameStateChanged gameStateChanged)
    {
        switch (gameStateChanged.State)
        {
            case GameState.PREBOSS:
            case GameState.WON:
            case GameState.LOST:
                foreach (var pool in pools.Values)
                {
                    pool.ReleaseObjects();
                }
                break;
        }
    }

    /// <summary>
    /// When called, this method returns an ammo object from the pool.
    /// </summary>
    /// <param name="ammoData">Ammo type</param>
    /// <returns></returns>
    public GameObject GetAmmo(AmmoData ammoData)
    {
        if (pools.TryGetValue(ammoData, out var pool))
        {
            return pool.GetObject();
        }

        Debug.LogError($"No pool found for AmmoData: {ammoData.name}");
        return null;
    }

    /// <summary>
    /// When called, this method releases an ammo object to the pool.
    /// </summary>
    /// <param name="ammoData"></param>
    /// <param name="ammo"></param>
    public void ReleaseAmmo(AmmoData ammoData, GameObject ammo)
    {
        if (pools.TryGetValue(ammoData, out var pool))
        {
            pool.ReleaseObject(ammo);
        }
        else
        {
            Debug.LogError($"No pool found for AmmoData: {ammoData.name}");
        }
    }
}
