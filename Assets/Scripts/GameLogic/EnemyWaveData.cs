using UnityEngine;

[System.Serializable]
public class EnemyWaveData
{
    public GameObject enemyPrefab;          // Prefab of the enemy(ies) that needs to spawn this wave
    public int maxEnemies = 10;             // Max number of enemies alive at once during this wave
    public float spawnInterval = 0.5f;      // Spawn interval
    public float waveDuration = 60f;        // Duration of the wave
}
