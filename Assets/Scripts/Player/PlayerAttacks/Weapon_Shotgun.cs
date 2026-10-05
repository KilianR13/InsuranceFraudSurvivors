using UnityEngine;

public class Weapon_Shotgun : Equippable
{
    [Header("Stats")]
    [SerializeField] private float damage = 4f;
    [SerializeField] private float attackCooldown = 1.5f;
    [SerializeField] private int bulletCount = 5;
    [SerializeField] private float spreadAngle = 30f;
    [SerializeField] private float bulletSpeed = 15f;
    [SerializeField] private float bulletLifetime = 10f;
    [SerializeField] private int maxEnemyPenetration = 0;

    [Header("References")]
    [SerializeField] private GameObject bulletPrefab;

    private float cooldownTimer;

    private void Update()
    {
        cooldownTimer -= Time.deltaTime;

        if (cooldownTimer <= 0f)
        {
            Shoot();
            cooldownTimer = attackCooldown * PlayerGlobalStats.Instance.CooldownMultiplier;
        }
    }

    private void Shoot()
    {
        for (int i = 0; i < bulletCount; i++)
        {
            float t = (float)i / (bulletCount - 1);

            float angle = Mathf.Lerp(
                -spreadAngle / 2f,
                spreadAngle / 2f,
                t
            );

            Vector2 direction = Quaternion.Euler(0f, 0f, angle) * transform.up;

            GameObject bulletObject = Instantiate(
                bulletPrefab,
                transform.position,
                Quaternion.identity
            );

            ProjectilePrefab bullet = bulletObject.GetComponent<ProjectilePrefab>();
            if (bullet != null)
            {
                bullet.damage = Mathf.RoundToInt(damage * PlayerGlobalStats.Instance.DamageMultiplier);
                bullet.speed = bulletSpeed * PlayerGlobalStats.Instance.ProjectileSpeedMultiplier;
                bullet.maxEnemyPenetration = maxEnemyPenetration + PlayerGlobalStats.Instance.ProjectilePenetration;
                bullet.lifetime = bulletLifetime;
                bullet.SetDirection(direction);
            }
        }
    }

    protected override void ApplyUpgrade()
    {
        switch (currentLevel)
        {
            case 2:
                break;

            case 3:
                break;

            case 4:
                break;

            case 5:
                break;

            case 6:
                break;

            case 7:
                break;

            case 8:
                break;

            case 9:
                break;
            default:
                Debug.LogError("Wtf?");
                break;
        }
    }
}
