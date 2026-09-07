using UnityEngine;

public class Weapon_Fireball : MonoBehaviour
{
    [Header("Base stats")]
    public float attackRange = 5f;
    public float attackCooldown = 1.5f;

    [Header("References")]
    public GameObject fireballPrefab;

    [Header("UpgradedStats")]
    public int bonusDamage = 0;
    public float bonusSpeed = 0f;

    private float cooldownTimer = 0f;

    private void Update()
    {
        cooldownTimer -= Time.deltaTime;

        if (cooldownTimer <= 0f)
        {
            EnemyAI nearestEnemy = FindNearestEnemy();

            if (nearestEnemy != null)
            {
                Shoot(nearestEnemy);
                cooldownTimer = attackCooldown;
            }
        }
    }

    private EnemyAI FindNearestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        EnemyAI nearest = null;
        float minDistance = Mathf.Infinity;

        foreach (GameObject enemyObject in enemies)
        {
            if (!enemyObject.activeInHierarchy)
                continue;

            float distance = Vector3.Distance(transform.position, enemyObject.transform.position);

            if (distance <= attackRange && distance < minDistance)
            {
                minDistance = distance;
                nearest = enemyObject.GetComponent<EnemyAI>();
            }
        }

        return nearest;
    }

    private void Shoot(EnemyAI enemy)
    {
        if (fireballPrefab == null)
            return;

        Vector3 direction = (enemy.transform.position - transform.position).normalized;

        GameObject fireballObject = Instantiate(
            fireballPrefab,
            transform.position,
            Quaternion.identity
        );

        FireBall_Prefab fireball = fireballObject.GetComponent<FireBall_Prefab>();

        if (fireball != null)
        {
            fireball.damage += bonusDamage;
            fireball.speed += bonusSpeed;
            fireball.SetDirection(direction);
        }
    }
}