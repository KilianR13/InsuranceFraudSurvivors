using UnityEngine;
using System.Collections.Generic;

public class EnemyWaveManager : MonoBehaviour
{
    [System.Serializable]
    public class EnemyEntry
    {
        public GameObject prefab;
        public int chances; // Chance of this enemy instance being chosen randomly to be spawned
    }

    [System.Serializable]
    public class Wave
    {
        public List<EnemyEntry> enemies = new List<EnemyEntry>(); 
        public int maxEnemiesAlive;
        public float spawnInterval;
        public float duration;
    }

    public Transform player;
    public List<Wave> waves = new List<Wave>();

    public int currentWaveIndex = 0;
    private float waveTimer = 0f;
    private float spawnTimer = 0f;
    private List<GameObject> activeEnemies = new List<GameObject>();
    private bool waveFinished = false;

    public GameObject music;

    void Start()
    {
        SimplePool.ClearAll();
        GameManager.gm.StartGame(music.GetComponent<AudioSource>());
    }

    public GameObject SpawnEnemy(GameObject prefab, Vector3 position, int waveId)
    {
        GameObject enemy = SimplePool.Get(prefab, position, Quaternion.identity);

        // Gets the Pool Identity ID
        PoolIdentity id = enemy.GetComponent<PoolIdentity>();
        if (id == null) id = enemy.AddComponent<PoolIdentity>();

        id.prefab = prefab;     // Original Prefab
        id.waveId = waveId;     // Wave it belongs to

        // The AI gets assigned a reference of the player.
        var ai = enemy.GetComponent<EnemyAI>();
        if (ai != null)
            ai.player = player;

        return enemy;
    }

    void Update()
    {
        if (waves.Count == 0) return;

        // Fallback in case the rest of the code to go back doesn't work
        if (waveFinished) return;

        Wave currentWave = waves[currentWaveIndex];
        waveTimer += Time.deltaTime;
        spawnTimer += Time.deltaTime;

        // Removes enemies that are null or that aren't active already
        activeEnemies.RemoveAll(e => e == null || !e.activeSelf);
        int aliveFromThisWave = activeEnemies.Count;

        // Starts spawning enemies if the max enemy cap isn't met
        if (aliveFromThisWave < currentWave.maxEnemiesAlive && spawnTimer >= currentWave.spawnInterval)
        {
            spawnTimer = 0f;
            Vector3 spawnPos = GetSpawnPositionOutsideCamera(10f);
            GameObject prefabToSpawn = ChooseEnemyPrefab(currentWave);
            var enemy = SpawnEnemy(prefabToSpawn, spawnPos, currentWaveIndex);


            EnemyAI ai = enemy.GetComponent<EnemyAI>();
            if (ai != null)
            {
                ai.waveIndex = currentWaveIndex;
                ai.poolable = true;
            }

            activeEnemies.Add(enemy);
        }

        // Checks if the timer for the current wave has finished or not
        if (waveTimer >= currentWave.duration)
        {
            waveFinished = true;

            // All enemies alive from that wave are marked as non-poolable, to be deleted later when they die
            foreach (var e in activeEnemies)
            {
                if (e != null && e.TryGetComponent(out EnemyAI ai))
                {
                    ai.poolable = false;
                }
            }

            // Clears the pool
            ClearWaveEnemies(currentWaveIndex);

            Debug.Log($"Wave {currentWaveIndex} finished");

            // If there's another wave, advances to the next one.
            if (waves.Count > currentWaveIndex + 1)
            {
                currentWaveIndex++;
                waveFinished = false;
                waveTimer = 0f;
                spawnTimer = 0f;
                activeEnemies.Clear();
            }
            else // If not, stops the game. Should polish this and make it like in Vampire Survivors, with an invincible enemy.
            {
                PlayerMovement_Car playerMovement = player.GetComponent<PlayerMovement_Car>();
                playerMovement.StopAllCoroutines();
                playerMovement.SilenceAllSound();
                GameManager.gm.StageCompleted(true);
            }
        }
    }

    private GameObject ChooseEnemyPrefab(Wave wave)
    {
        int total = 0;
        foreach (var e in wave.enemies) // Counts how many chances available are for enemies to spawn in this wave
            total += e.chances;

        int r = Random.Range(0, total); // Picks a random number between 0 and the number of chances total
        int sum = 0;

        foreach (var e in wave.enemies)
        {
            sum += e.chances;
            if (r < sum)
                return e.prefab; // Chooses randomly an enemy
        }

        return wave.enemies[0].prefab; // Fallback
    }

    // Function to mark and remove enemies from previous waves
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

        foreach (var obj in toRemove)
        {
            PoolIdentity id = obj.GetComponent<PoolIdentity>();
            SimplePool.RemoveSpecific(id.prefab, obj);
            Destroy(obj);
        }
    }

    // Function to spawn the enemies outside the player's field of view
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
