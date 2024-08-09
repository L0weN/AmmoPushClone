using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is responsible for managing a pool of objects.
/// </summary>
public class ObjectPool : MonoBehaviour
{
    private List<GameObject> pool; // List of objects in the pool.
    private GameObject prefab; // Prefab of the object to be pooled.

    /// <summary>
    /// Initializes the object pool.
    /// </summary>
    /// <param name="prefab"></param>
    /// <param name="initialPoolSize"></param>
    public void Initialize(GameObject prefab, int initialPoolSize)
    {
        this.prefab = prefab;
        pool = new List<GameObject>();

        for (int i = 0; i < initialPoolSize; i++)
        {
            GameObject obj = Instantiate(prefab, transform);
            obj.SetActive(false);
            pool.Add(obj);
        }
    }

    /// <summary>
    /// When called, this method returns an object from the pool.
    /// </summary>
    /// <returns></returns>
    public GameObject GetObject()
    {
        foreach (var obj in pool)
        {
            if (!obj.activeInHierarchy)
            {
                obj.SetActive(true);
                return obj;
            }
        }

        GameObject newObj = Instantiate(prefab , transform);
        pool.Add(newObj);
        return newObj;
    }

    /// <summary>
    /// When called, this method returns an object from the pool.
    /// </summary>
    /// <param name="obj"></param>
    public void ReleaseObject(GameObject obj)
    {
        obj.SetActive(false);
    }

    /// <summary>
    /// When called, this method releases all objects in the pool.
    /// </summary>
    public void ReleaseObjects()
    {
        foreach (var obj in pool)
        {
            if (obj.activeInHierarchy)
            {
                ReleaseObject(obj);
            }
        }
    }
}
