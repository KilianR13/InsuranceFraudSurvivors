using UnityEngine;

public class ProjectileShoot : MonoBehaviour
{
    public static void SimpleShoot(
        Vector2 origin,
        Vector2 velocity,
        BulletPool bulletPool)
    {
        Bullet bullet = bulletPool.RequestBullet();

        bullet.transform.position = origin;
        bullet.Velocity = velocity;
    }

    public static void RadialShoot
        (Vector2 origin, Vector2 aimDirection, BulletPatternSettings settings, BulletPool bulletPool)
    {
        float angleBetweenBullets = 360f / settings.NumberOfBullets;

        if (settings.AngleOffset != 0f || settings.PhaseOffset != 0f)
        {
            aimDirection = aimDirection
                .Rotate(settings.AngleOffset + (settings.PhaseOffset * angleBetweenBullets));
        }

        for (int i = 0; i < settings.NumberOfBullets; i++)
        {
            float bulletDirectionAngle = angleBetweenBullets * i;

            if (settings.RadialMask && bulletDirectionAngle > settings.MaskAngle)
            {
                break;
            }

            Vector2 bulletDirection = aimDirection.Rotate(bulletDirectionAngle);
            SimpleShoot(origin, bulletDirection * settings.BulletSpeed, bulletPool);
        }
    }
}
