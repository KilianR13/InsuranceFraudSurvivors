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
        // float respawnRadius = 40f;
        // int attemptsPerEnemy = 30;

        // Debug.Log($"Respawning {activeEnemies.Count} enemies via pool around player at {player.position}");

        // List<GameObject> oldActive = new List<GameObject>(activeEnemies);
        // List<GameObject> newActive = new List<GameObject>();

        // // Limpiamos la lista original (vamos a rellenarla con las nuevas instancias)
        // // No la vaciamos de inmediato para evitar problemas si algo lee la lista durante el proceso.
        // activeEnemies.Clear();

        // foreach (var old in oldActive)
        // {
        //     if (old == null) continue;

        //     // Obtener prefab original (PoolIdentity preferido, sino Poolable)
        //     GameObject prefab = null;
        //     if (old.TryGetComponent<PoolIdentity>(out var id))
        //         prefab = id.prefab;
        //     else if (old.TryGetComponent<Poolable>(out var p))
        //         prefab = p.originalPrefab;

        //     if (prefab == null)
        //     {
        //         Debug.LogWarning($"No pude obtener prefab del enemigo {old.name}, destruyéndolo.");
        //         Destroy(old);
        //         continue;
        //     }

        //     // Devolver el antiguo al pool (o desactivarlo según tu SimplePool)
        //     // Esto debe corresponder con la forma en que tu pool espera devolver objetos.
        //     // EnemyAI.killEnemy usa SimplePool.Return(p.originalPrefab, gameObject);
        //     SimplePool.Return(prefab, old);

        //     // Generar una posición válida alrededor del jugador (intenta varios veces)
        //     Vector3 spawnPos = Vector3.zero;
        //     bool found = false;
        //     for (int a = 0; a < attemptsPerEnemy; a++)
        //     {
        //         Vector2 offset = Random.insideUnitCircle * respawnRadius;
        //         Vector3 candidate = player.position + new Vector3(offset.x, offset.y, 0f);

        //         // (Opcional) comprobar no spawnear dentro de la cámara / dentro de obstáculos:
        //         // si quieres, aquí puedes poner un Physics2D.OverlapCircle para evitar colisiones.
        //         spawnPos = candidate;
        //         found = true;
        //         break;
        //     }

        //     if (!found) spawnPos = player.position; // fallback

        //     // Spawn inmediato usando tu spawner (usa el pool internamente)
        //     GameObject spawned = spawner.SpawnEnemy(prefab, spawnPos, currentWaveIndex);

        //     if (spawned == null)
        //     {
        //         Debug.LogWarning($"Spawner devolvió null para prefab {prefab.name}");
        //         continue;
        //     }

        //     // Configurar AI / flags como al spawnear normalmente
        //     if (spawned.TryGetComponent<EnemyAI>(out var ai))
        //     {
        //         ai.waveIndex = currentWaveIndex;
        //         ai.poolable = true;
        //         ai.player = player;
        //         // Si quieres forzar una recalculación inmediata en la IA:
        //         // ai.OnRespawn(); // si implementas ese método opcional en EnemyAI
        //     }

        //     newActive.Add(spawned);
        // }

        // // Sustituir la lista de enemigos activos por las nuevas instancias
        // activeEnemies = newActive;

        // Debug.Log($"Respawn completo. Ahora hay {activeEnemies.Count} enemigos activos alrededor del jugador.");

        float respawnRadius = 40f;
        int attemptsPerEnemy = 30;

        Debug.Log($"Respawning {activeEnemies.Count} enemies via pool around player at {player.position}");

        List<GameObject> oldActive = new List<GameObject>(activeEnemies);
        List<GameObject> newActive = new List<GameObject>();

        // Limpiamos la lista original
        activeEnemies.Clear();

        // Obtener referencias del tamaño del mapa desde WorldWrapper
        WorldWrapper wrapper = player.GetComponent<WorldWrapper>();
        float halfWidth = wrapper != null ? wrapper.mapWidth * 0.5f : 432f;   // fallback
        float halfHeight = wrapper != null ? wrapper.mapHeight * 0.5f : 378f; // fallback

        foreach (var old in oldActive)
        {
            if (old == null) continue;

            // Obtener prefab original (PoolIdentity preferido, sino Poolable)
            GameObject prefab = null;
            if (old.TryGetComponent<PoolIdentity>(out var id))
                prefab = id.prefab;
            else if (old.TryGetComponent<Poolable>(out var p))
                prefab = p.originalPrefab;

            if (prefab == null)
            {
                Debug.LogWarning($"No pude obtener prefab del enemigo {old.name}, destruyéndolo.");
                Destroy(old);
                continue;
            }

            // Devolver el antiguo al pool
            SimplePool.Return(prefab, old);

            // --- Recentrar al jugador respecto al mapa toroidal ---
            Vector3 playerPos = player.position;
            if (playerPos.x > halfWidth) playerPos.x -= wrapper.mapWidth;
            else if (playerPos.x < -halfWidth) playerPos.x += wrapper.mapWidth;

            if (playerPos.y > halfHeight) playerPos.y -= wrapper.mapHeight;
            else if (playerPos.y < -halfHeight) playerPos.y += wrapper.mapHeight;

            // Generar una posición válida alrededor del jugador
            Vector3 spawnPos = Vector3.zero;
            bool found = false;
            for (int a = 0; a < attemptsPerEnemy; a++)
            {
                Vector2 offset = Random.insideUnitCircle * respawnRadius;
                spawnPos = playerPos + new Vector3(offset.x, offset.y, 0f);
                found = true;
            }

            if (!found) spawnPos = playerPos; // fallback

            // Spawn inmediato usando tu spawner (usa el pool internamente)
            GameObject spawned = spawner.SpawnEnemy(prefab, spawnPos, currentWaveIndex);

            if (spawned == null)
            {
                Debug.LogWarning($"Spawner devolvió null para prefab {prefab.name}");
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

        Debug.Log($"Respawn completo. Ahora hay {activeEnemies.Count} enemigos activos alrededor del jugador.");

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
