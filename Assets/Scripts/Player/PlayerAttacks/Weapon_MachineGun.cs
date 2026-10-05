using System.Collections;
using UnityEngine;

public class Weapon_MachineGun : Equippable
{
    [Header("Stats")]
    [SerializeField] private float damage = 1f;
    [SerializeField] private float burstCooldown = 1.5f;
    [SerializeField] private int shotsPerBurst = 5;
    [SerializeField] private float shotInterval = 0.1f;
    [SerializeField] private float bulletSpeed = 15f;
    [SerializeField] private float bulletLifetime = 10f;
    [SerializeField] private int maxEnemyPenetration = 0;

    [Header("References")]
    [SerializeField] private GameObject bulletPrefab;

    private bool isShooting = false;

    private void Update()
    {
        if (!isShooting)
        {
            StartCoroutine(ShootBurst());
        }
    }

    private IEnumerator ShootBurst()
    {
        isShooting = true;

        for (int i = 0; i < shotsPerBurst; i++)
        {
            Shoot();

            if (i < shotsPerBurst - 1)
            {
                yield return new WaitForSeconds(shotInterval);
            }
        }

        yield return new WaitForSeconds(
            burstCooldown * PlayerGlobalStats.Instance.CooldownMultiplier
        );

        isShooting = false;
    }

    private void Shoot()
    {
        GameObject bulletObject = Instantiate(
            bulletPrefab,
            transform.position,
            Quaternion.identity
        );

        bulletObject.transform.localScale *= PlayerGlobalStats.Instance.projectileSizeMultiplier;

        ProjectilePrefab bullet = bulletObject.GetComponent<ProjectilePrefab>();

        if (bullet != null)
        {
            bullet.damage = Mathf.RoundToInt(damage * PlayerGlobalStats.Instance.DamageMultiplier);
            bullet.speed = bulletSpeed * PlayerGlobalStats.Instance.ProjectileSpeedMultiplier;
            bullet.maxEnemyPenetration = maxEnemyPenetration + PlayerGlobalStats.Instance.ProjectilePenetration;
            bullet.lifetime = bulletLifetime;
            bullet.SetDirection(transform.up);
        }
    }

    protected override void ApplyUpgrade()
    {
        switch (currentLevel)
        {
            case 1:
                break;

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
        }
    }
}
