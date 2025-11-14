using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] enemyPrefabs;
    public Transform player;

    public GameObject SpawnEnemy(GameObject prefab, Vector3 position, int waveId)
    {
        GameObject enemy = SimplePool.Get(prefab, position, Quaternion.identity);

        // Guardar prefab original en Poolable (tu código)
        Poolable p = enemy.GetComponent<Poolable>();
        if (p == null) p = enemy.AddComponent<Poolable>();
        p.originalPrefab = prefab;

        // NUEVO — Identidad del pool con la WAVE ID
        PoolIdentity id = enemy.GetComponent<PoolIdentity>();
        if (id == null) id = enemy.AddComponent<PoolIdentity>();

        id.prefab = prefab;     // Prefab original
        id.waveId = waveId;     // Oleada a la que pertenece

        // Asignar AI
        var ai = enemy.GetComponent<EnemyAI>();
        if (ai != null)
            ai.player = player;

        return enemy;
    }
}
