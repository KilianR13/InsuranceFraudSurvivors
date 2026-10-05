using System.Collections;
using UnityEngine;

public class RadialShootingWeapon : MonoBehaviour
{
    [SerializeField] private BulletPattern _shootPattern;
    [SerializeField] private BulletPool bulletPool;
    private bool _isShooting = false;

    void Update()
    {
        if (_isShooting)
        {
            return;
        }
        StartCoroutine(ExecuteRadialShotPattern(_shootPattern));
    }

    private IEnumerator ExecuteRadialShotPattern(BulletPattern pattern)
    {
        _isShooting = true;

        int lap = 0;
        Vector2 aimDirection = transform.up;
        Vector2 center = transform.position;

        yield return new WaitForSeconds(pattern.StartWait);

        while (lap < pattern.Repetitions)
        {
            if (lap > 0 && pattern.AngleOffsetBetweenReps != 0f)
            {
                aimDirection = aimDirection.Rotate(pattern.AngleOffsetBetweenReps);
            }
            
            for (int i = 0; i < pattern.PatternSettings.Length; i++)
            {
                ProjectileShoot.RadialShoot(center, aimDirection, pattern.PatternSettings[i], bulletPool);
                yield return new WaitForSeconds(pattern.PatternSettings[i].CooldownAfterShot);
            }
            lap++;    
        }

        yield return new WaitForSeconds(pattern.EndWait);

        _isShooting = false;
    }
}
