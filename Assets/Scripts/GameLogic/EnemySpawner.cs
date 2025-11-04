using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] enemyPrefabs;
    public Transform player;

    public GameObject SpawnEnemy(GameObject prefab, Vector3 position)
    {
        GameObject enemy = SimplePool.Get(prefab, position, Quaternion.identity);

        var ai = enemy.GetComponent<EnemyAI>();
        if (ai != null)
            ai.player = player;

        return enemy;
    }
}
