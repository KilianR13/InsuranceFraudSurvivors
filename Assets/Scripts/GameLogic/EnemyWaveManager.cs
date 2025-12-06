using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class EnemyWaveManager : MonoBehaviour
{
    [System.Serializable]
    public class EnemyEntry
    {
        public GameObject prefab;
        public int amount; // cuántos debe haber en esta oleada
    }

    [System.Serializable]
    public class Wave
    {
        public string name;
        public List<EnemyEntry> enemies = new List<EnemyEntry>(); 
        // Ejemplo en el inspector:
        //  - prefab Giulia | amount = 10
        //  - prefab Zombie | amount = 5
        public int maxEnemiesAlive;
        public float spawnInterval;
        public float duration;
    }

    public Transform player;
    public EnemySpawner spawner;
    public List<Wave> waves = new List<Wave>();

    public int currentWaveIndex = 0;
    private float waveTimer = 0f;
    private float spawnTimer = 0f;
    private List<GameObject> activeEnemies = new List<GameObject>();
    private bool waveFinished = false;

    void Start()
    {
        if (player != null)
        {
            var wrap = player.GetComponent<WorldWrapper>();
            if (wrap != null)
            {
                wrap.changedSides -= RespawnEnemiesAroundPlayer; // evita dobles suscripciones
                wrap.changedSides += RespawnEnemiesAroundPlayer;
            }
        }
    }

    public void RespawnEnemiesAroundPlayer()
    {
        Debug.Log("Respawning enemigos alrededor del jugador");
        
        float respawnRadius = 40f;

        foreach (var enemy in activeEnemies)
        {
            if (enemy == null) continue;
            // Desactivar temporalmente para resetear correctamente
            enemy.SetActive(false);

            // Reubicar cerca del jugador
            Vector3 newPos = player.position + Random.insideUnitSphere * respawnRadius;
            enemy.transform.position = new Vector3(newPos.x, newPos.y, 0f);

            // Resetear velocidad si tiene Rigidbody2D
            if (enemy.TryGetComponent(out Rigidbody2D rb))
            {
                rb.linearVelocity = Vector2.zero;
                rb.angularVelocity = 0f;
            }

            // Reactivar
            enemy.SetActive(true);
        }
    }

    void Update()
    {
        if (waves.Count == 0 || spawner == null) return;

        // Detener spawn si la oleada terminó
        if (waveFinished) return;

        Wave currentWave = waves[currentWaveIndex];
        waveTimer += Time.deltaTime;
        spawnTimer += Time.deltaTime;

        // Limpia enemigos muertos de esta oleada
        activeEnemies.RemoveAll(e => e == null || !e.activeSelf);
        int aliveFromThisWave = activeEnemies.Count;

        // Spawn enemigos mientras haya espacio
        if (aliveFromThisWave < currentWave.maxEnemiesAlive && spawnTimer >= currentWave.spawnInterval)
        {
            spawnTimer = 0f;
            Vector3 spawnPos = GetSpawnPositionOutsideCamera(10f);
            GameObject prefabToSpawn = ChooseEnemyPrefab(currentWave);
            var enemy = spawner.SpawnEnemy(prefabToSpawn, spawnPos, currentWaveIndex);


            EnemyAI ai = enemy.GetComponent<EnemyAI>();
            if (ai != null)
            {
                ai.waveIndex = currentWaveIndex;
                ai.poolable = true;
            }

            activeEnemies.Add(enemy);
        }

        // Comprobar fin de oleada
        if (waveTimer >= currentWave.duration)
        {
            waveFinished = true;

            // Marcar enemigos vivos como no poolable
            foreach (var e in activeEnemies)
            {
                if (e != null && e.TryGetComponent(out EnemyAI ai))
                {
                    ai.poolable = false;
                }
            }

            // Limpiar enemigos muertos del pool
            ClearWaveEnemies(currentWaveIndex);

            Debug.Log($"Oleada {currentWaveIndex} terminada");

            // Avanzar a la siguiente oleada si existe
            if (waves.Count > currentWaveIndex + 1)
            {
                currentWaveIndex++;
                waveFinished = false;
                waveTimer = 0f;
                spawnTimer = 0f;
                activeEnemies.Clear();
            }
            else
            {
                GameManager.gm.StageCompleted();
            }
        }
    }

    private GameObject ChooseEnemyPrefab(Wave wave)
    {
        int total = 0;
        foreach (var e in wave.enemies)
            total += e.amount;

        int r = Random.Range(0, total);
        int sum = 0;

        foreach (var e in wave.enemies)
        {
            sum += e.amount;
            if (r < sum)
                return e.prefab;
        }

        return wave.enemies[0].prefab; // fallback
    }


    private void ClearWaveEnemies(int waveIndex)
    {
        List<GameObject> toRemove = new();

        var dict = SimplePool.GetInternalDictionary(); // necesitas exponer este método en SimplePool

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

        // Destruir físicamente y sacar del pool
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
