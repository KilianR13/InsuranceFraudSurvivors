using UnityEngine;

public class Weapon_SniperRifle : Equippable
{
    [Header("Base stats")]
    [SerializeField] private int damage = 1;
    [SerializeField] private float bulletSpeed = 10f;
    [SerializeField] private float firingCooldown;
    [SerializeField] private float firingRange;
    [SerializeField] private int maxEnemyPenetration = 1;

    [Header("References")]
    public GameObject bulletPrefab;
    public Transform firingPoint;
    public LineRenderer laser;

    private float cooldownTimer = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    private void Update()
    {
        cooldownTimer -= Time.deltaTime;

        EnemyAI nearestEnemy = FindNearestEnemy();

        if (nearestEnemy != null)
        {
            Vector3 direction = nearestEnemy.transform.position - transform.position;

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            transform.rotation = Quaternion.Euler(0f, 0f, angle - 90f);

            laser.SetPosition(0, firingPoint.position);
            laser.SetPosition(1, nearestEnemy.transform.position);
            if (cooldownTimer <= 0f)
            {
                Shoot(nearestEnemy);
                cooldownTimer = firingCooldown * PlayerGlobalStats.Instance.CooldownMultiplier;
            }
        }
        else
        {
            laser.SetPosition(0, firingPoint.position);
            laser.SetPosition(1, firingPoint.position);
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

            if (distance <= firingRange && distance < minDistance)
            {
                minDistance = distance;
                nearest = enemyObject.GetComponent<EnemyAI>();
            }
        }

        return nearest;
    }

    private void Shoot(EnemyAI enemy)
    {
        if (bulletPrefab == null)
            return;

        Vector3 direction = (enemy.transform.position - firingPoint.position).normalized;

        GameObject bulletObject = Instantiate(
            bulletPrefab,
            firingPoint.position,
            Quaternion.identity
        );

        ProjectilePrefab projectile = bulletObject.GetComponent<ProjectilePrefab>();

        if (projectile != null)
        {
            projectile.damage = Mathf.RoundToInt(damage * PlayerGlobalStats.Instance.DamageMultiplier);
            projectile.speed = bulletSpeed * PlayerGlobalStats.Instance.ProjectileSpeedMultiplier;
            projectile.maxEnemyPenetration = maxEnemyPenetration + PlayerGlobalStats.Instance.ProjectilePenetration;
            projectile.SetDirection(direction);
        }
    }

    protected override void ApplyUpgrade()
    {
        switch (currentLevel)
        {
            case 2:
                // baseDamage += 3;
                break;
            case 3:
                // damageMultiplier += 0.2f;
                break;
            case 4:
                // baseDamage += 3;
                break;
            case 5:
                // baseDamage += 6;
                break;
            case 6:
                // damageMultiplier += 0.5f;
                break;
            case 7:
                // baseDamage += 6;
                break;
            case 8:
                //
                break;
            case 9:
                //
                break;
            default:
                Debug.LogError("Wtf?");
                break;
        }
    }
}
