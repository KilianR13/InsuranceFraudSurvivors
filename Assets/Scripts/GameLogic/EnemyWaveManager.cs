using UnityEngine;
using System.Collections.Generic;

public class EnemyWaveManager : MonoBehaviour
{
    [System.Serializable]
    public class Wave
    {
        public string name;
        public GameObject enemyPrefab;
        public int maxEnemiesAlive;
        public float spawnInterval;
        public float duration;
    }

    public Transform player;
    public EnemySpawner spawner;
    public List<Wave> waves = new List<Wave>();

    private int currentWaveIndex = 0;
    private float waveTimer = 0f;
    private float spawnTimer = 0f;
    private List<GameObject> activeEnemies = new List<GameObject>();

    void Update()
    {
        if (waves.Count == 0 || spawner == null) return;

        Wave currentWave = waves[currentWaveIndex];
        waveTimer += Time.deltaTime;
        spawnTimer += Time.deltaTime;

        // Limpia SOLO enemigos muertos de esta oleada
        activeEnemies.RemoveAll(e => e == null || !e.activeSelf);

        int aliveFromThisWave = activeEnemies.Count;

        // Spawn de enemigos
        if (aliveFromThisWave < currentWave.maxEnemiesAlive && spawnTimer >= currentWave.spawnInterval)
        {
            spawnTimer = 0f;

            Vector3 spawnPos = GetSpawnPositionOutsideCamera(10f);

            // PASAMOS LA OLEADA AQUÍ
            var enemy = spawner.SpawnEnemy(currentWave.enemyPrefab, spawnPos, currentWaveIndex);

            activeEnemies.Add(enemy);
        }

        // Cambiar de oleada
        if (waveTimer >= currentWave.duration)
        {
            foreach (var e in activeEnemies)
            {
                if (e != null && e.TryGetComponent(out EnemyAI ai))
                {
                    ai.poolable = false;
                }
            }
            // Limpia enemigos del pool de oleadas anteriores
            ClearWaveEnemies(currentWaveIndex);

            waveTimer = 0f;
            activeEnemies.Clear();
            spawnTimer = 0f;

            currentWaveIndex = (currentWaveIndex + 1) % waves.Count;
        }
    }

    // 🔥 Limpia enemigos (vivos o muertos) de una oleada pasadas
    private void ClearWaveEnemies(int waveIndex)
    {
        List<GameObject> toRemove = new();

        var dict = SimplePool.GetInternalDictionary();

        foreach (var kvp in dict)
        {
            foreach (var obj in kvp.Value)
            {
                if (obj == null) continue;

                if (obj.TryGetComponent(out PoolIdentity id))
                {
                    if (id.waveId == waveIndex)
                        toRemove.Add(obj);
                }
            }
        }

        // destruir físicamente y sacar del pool
        foreach (var obj in toRemove)
        {
            PoolIdentity id = obj.GetComponent<PoolIdentity>();
            SimplePool.RemoveSpecific(id.prefab, obj);
            Destroy(obj);
        }
    }


    private Vector3 GetSpawnPositionOutsideCamera(float minDistanceFromPlayer)
    {
        Camera cam = Camera.main;
        Vector3 spawnPos;
        int attempts = 0;

        float camHeight = 2f * cam.orthographicSize;
        float camWidth = camHeight * cam.aspect;

        float left = cam.transform.position.x - camWidth / 2f;
        float right = cam.transform.position.x + camWidth / 2f;
        float bottom = cam.transform.position.y - camHeight / 2f;
        float top = cam.transform.position.y + camHeight / 2f;

        do
        {
            float x, y;

            if (Random.value < 0.5f)
            {
                x = Random.value < 0.5f ? left - minDistanceFromPlayer : right + minDistanceFromPlayer;
                y = Random.Range(bottom - minDistanceFromPlayer, top + minDistanceFromPlayer);
            }
            else
            {
                x = Random.Range(left - minDistanceFromPlayer, right + minDistanceFromPlayer);
                y = Random.value < 0.5f ? bottom - minDistanceFromPlayer : top + minDistanceFromPlayer;
            }

            spawnPos = new Vector3(x, y, 0f);
            attempts++;

        } while (Vector3.Distance(spawnPos, player.position) < minDistanceFromPlayer && attempts < 100);

        return spawnPos;
    }
}
