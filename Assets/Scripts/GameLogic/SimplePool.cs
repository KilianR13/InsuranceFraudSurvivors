using System.Collections.Generic;
using UnityEngine;

public static class SimplePool
{
    private static Dictionary<GameObject, Queue<GameObject>> poolDict = new();

    /// <summary>
    /// Crea un pool inicial si no existe.
    /// </summary>
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
    /// Obtiene un objeto del pool o crea uno nuevo si no hay disponibles.
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
    /// Devuelve un objeto al pool (lo desactiva).
    /// </summary>
    public static void Return(GameObject prefab, GameObject obj)
    {
        if (!poolDict.ContainsKey(prefab))
            poolDict[prefab] = new Queue<GameObject>();

        obj.SetActive(false);
        poolDict[prefab].Enqueue(obj);
    }

    /// <summary>
    /// Limpia completamente un pool específico.
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
}
