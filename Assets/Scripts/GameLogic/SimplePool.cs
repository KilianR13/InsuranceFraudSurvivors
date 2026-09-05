using System.Collections.Generic;
using UnityEngine;

public static class SimplePool
{
    // Prefab dictionary 
    private static Dictionary<GameObject, Queue<GameObject>> poolDict = new();

    
    /// <summary>
    /// Creates the pool if there isn't one already.
    /// </summary>
    /// <param name="prefab">Original prefab of the enemy.</param>
    /// <param name="amount">How many enemies are to be instantiated</param>
    public static void Prewarm(GameObject prefab, int amount)
    {
        if (!poolDict.ContainsKey(prefab))
            poolDict[prefab] = new Queue<GameObject>();

        for (int i = 0; i < amount; i++)
        {
            GameObject obj = Object.Instantiate(prefab);
            obj.SetActive(false);
            poolDict[prefab].Enqueue(obj);
        }
    }

    /// <summary>
    /// Grabs a prefab from the queue and instantiates it in a set position with a set rotation.
    /// </summary>
    public static GameObject Get(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        if (!poolDict.ContainsKey(prefab))
            poolDict[prefab] = new Queue<GameObject>();

        GameObject obj;
        if (poolDict[prefab].Count > 0)
        {
            obj = poolDict[prefab].Dequeue();
            obj.SetActive(true);
        }
        else
        {
            obj = Object.Instantiate(prefab);
        }

        obj.transform.SetPositionAndRotation(position, rotation);
        return obj;
    }

    /// <summary>
    /// Returns an object to the pool, aka it deactivates it.
    /// </summary>
    public static void Return(GameObject prefab, GameObject obj)
    {
        if (!poolDict.ContainsKey(prefab))
            poolDict[prefab] = new Queue<GameObject>();

        obj.SetActive(false);
        poolDict[prefab].Enqueue(obj);
    }

    /// <summary>
    /// Deletes everything from a pool.
    /// </summary>
    public static void ClearPool(GameObject prefab)
    {
        if (!poolDict.ContainsKey(prefab)) return;

        foreach (var obj in poolDict[prefab])
        {
            if (obj != null)
                Object.Destroy(obj);
        }

        poolDict[prefab].Clear();
    }

    /// <summary>
    /// Deletes everything inside the pool Dictionary.
    /// </summary>
    public static void ClearAll()
    {
        poolDict.Clear();
    }

    /// <summary>
    /// Obtain the pool Dictionary.
    /// </summary>
    /// <returns>Pool Dictionary</returns>
    public static Dictionary<GameObject, Queue<GameObject>> GetInternalDictionary()
    {
        return poolDict;
    }

    /// <summary>
    /// Remove a specific prefab (enemy) from the queue by rebuilding the queue.
    /// </summary>
    /// <param name="prefab">Prefab ID</param>
    /// <param name="obj">Whatever you want to remove from the queue</param>
    public static void RemoveSpecific(GameObject prefab, GameObject obj)
    {
        if (!poolDict.ContainsKey(prefab)) // If inside the Dictionary there isn't the prefab you want to remove, what's there to remove?
            return;

        Queue<GameObject> queue = poolDict[prefab];

        // Rebuild the queue without the specific object
        Queue<GameObject> newQueue = new Queue<GameObject>();

        foreach (var e in queue)
        {
            if (e != obj)
                newQueue.Enqueue(e);
        }

        poolDict[prefab] = newQueue;
    }


    
}
