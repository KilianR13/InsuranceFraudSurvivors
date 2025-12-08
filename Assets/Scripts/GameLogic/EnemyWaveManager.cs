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
        SimplePool.ClearAll();
        if (player != null)
        {
            var wrap = player.GetComponent<WorldWrapper>();
            if (wrap != null)
            {
                wrap.changedSides -= RespawnEnemiesAroundPlayer; // Evita dobles suscripciones
                wrap.changedSides += RespawnEnemiesAroundPlayer;
            }
        }
        GameManager.gm.StartGame();
    }

    public void RespawnEnemiesAroundPlayer()
    {
        float respawnRadius = 50f; // Espacio de seguridad para el jugador, para que le de tiempo a la cámara a llegar y el jugador pueda reaccionar a enemigos.
        int attemptsPerEnemy = 30;

        List<GameObject> oldActive = new List<GameObject>(activeEnemies);
        List<GameObject> newActive = new List<GameObject>();

        // Limpia la lista original. No es ideal, pero el propio pool respawneará los enemigos necesarios si se borran algunos inactivos.
        activeEnemies.Clear();

        // Obtiene referencias del tamaño del mapa desde WorldWrapper
        WorldWrapper wrapper = player.GetComponent<WorldWrapper>();
        float halfWidth = wrapper != null ? wrapper.mapWidth * 0.5f : 432f;   // fallback
        float halfHeight = wrapper != null ? wrapper.mapHeight * 0.5f : 378f; // fallback

        foreach (var old in oldActive)
        {
            if (old == null) // Si encuentra un valor nulo, se lo salta.
            {
                continue;
            }

            // Obtener prefab original (PoolIdentity preferido, y si no, Poolable)
            GameObject prefab = null;
            if (old.TryGetComponent<PoolIdentity>(out var id))
            {
                prefab = id.prefab;
            }
            else if (old.TryGetComponent<Poolable>(out var p))
            {
                prefab = p.originalPrefab;
            }

            // Si no puede obtener el prefab, destruye el objeto para evitar errores.
            if (prefab == null)
            {
                Destroy(old);
                continue;
            }

            // Devolver el antiguo al pool
            SimplePool.Return(prefab, old);

            // Recentra al 
            Vector3 playerPos = player.position;
            if (playerPos.x > halfWidth)
            {
                playerPos.x -= wrapper.mapWidth;  
            }
            else if (playerPos.x < -halfWidth)
            {
                playerPos.x += wrapper.mapWidth;  
            } 

            if (playerPos.y > halfHeight)
            {
                playerPos.y -= wrapper.mapHeight;
            } 
            else if (playerPos.y < -halfHeight)
            {
                playerPos.y += wrapper.mapHeight;
            }

            // Generar una posición válida alrededor del jugador
            Vector3 spawnPos = Vector3.zero;
            bool found = false;
            for (int a = 0; a < attemptsPerEnemy; a++)
            {
                Vector2 offset = Random.insideUnitCircle * respawnRadius;
                spawnPos = playerPos + new Vector3(offset.x, offset.y, 0f);
                found = true;
            }

            if (!found)
            {
                spawnPos = playerPos; // fallback  
            } 

            // Spawn inmediato usando tu spawner (usa el pool internamente)
            GameObject spawned = spawner.SpawnEnemy(prefab, spawnPos, currentWaveIndex);

            if (spawned == null)
            {
                continue;
            }

            // Configurar AI / flags como al spawnear normalmente
            if (spawned.TryGetComponent<EnemyAI>(out var ai))
            {
                ai.waveIndex = currentWaveIndex;
                ai.poolable = true;
                ai.player = player;
            }

            newActive.Add(spawned);
        }

        // Sustituir la lista de enemigos activos por las nuevas instancias
        activeEnemies = newActive;

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
                GameManager.gm.StageCompleted(true);
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
