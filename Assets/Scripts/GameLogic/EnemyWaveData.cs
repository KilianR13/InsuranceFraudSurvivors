using UnityEngine;

[System.Serializable]
public class EnemyWaveData
{
    public string waveName = "New Wave";
    public GameObject enemyPrefab; // Prefab de enemigo a spawnear
    public int maxEnemies = 10;    // Máximo de enemigos vivos en esta oleada
    public float spawnInterval = 0.5f; // Tiempo entre spawns
    public float waveDuration = 60f;   // Duración antes de pasar a la siguiente
}
