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

        // Limpia los enemigos inactivos
        activeEnemies.RemoveAll(e => e == null || !e.activeSelf);

        // Spawnea mientras haya hueco
        if (activeEnemies.Count < currentWave.maxEnemiesAlive && spawnTimer >= currentWave.spawnInterval)
        {
            spawnTimer = 0f;

            Vector3 spawnPos = player.position + (Vector3)(Random.insideUnitCircle.normalized * 10f);
            var enemy = spawner.SpawnEnemy(currentWave.enemyPrefab, spawnPos);
            activeEnemies.Add(enemy);
        }

        // Cambia de oleada si pasa el tiempo
        if (waveTimer >= currentWave.duration)
        {
            waveTimer = 0f;
            currentWaveIndex = (currentWaveIndex + 1) % waves.Count; // bucle infinito
        }
    }
}
