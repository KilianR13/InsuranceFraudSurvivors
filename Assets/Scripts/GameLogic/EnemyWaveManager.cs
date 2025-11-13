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

            Vector3 spawnPos = GetSpawnPositionOutsideCamera(10f);
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
    private Vector3 GetSpawnPositionOutsideCamera(float minDistanceFromPlayer)
    {
        Camera cam = Camera.main;
        Vector3 spawnPos;
        int attempts = 0;

        // Calcula los límites de la cámara en world space
        float camHeight = 2f * cam.orthographicSize;
        float camWidth = camHeight * cam.aspect;

        float left = cam.transform.position.x - camWidth / 2f;
        float right = cam.transform.position.x + camWidth / 2f;
        float bottom = cam.transform.position.y - camHeight / 2f;
        float top = cam.transform.position.y + camHeight / 2f;

        do
        {
            // Genera un punto aleatorio en un rectángulo más grande que la cámara
            float x, y;

            // Decide si spawnear horizontalmente o verticalmente fuera de la cámara
            if (Random.value < 0.5f)
            {
                // Izquierda o derecha
                x = Random.value < 0.5f ? left - minDistanceFromPlayer : right + minDistanceFromPlayer;
                y = Random.Range(bottom - minDistanceFromPlayer, top + minDistanceFromPlayer);
            }
            else
            {
                // Arriba o abajo
                x = Random.Range(left - minDistanceFromPlayer, right + minDistanceFromPlayer);
                y = Random.value < 0.5f ? bottom - minDistanceFromPlayer : top + minDistanceFromPlayer;
            }

            spawnPos = new Vector3(x, y, 0f);
            attempts++;

        } while (Vector3.Distance(spawnPos, player.position) < minDistanceFromPlayer && attempts < 100);

        return spawnPos;
    }


}
