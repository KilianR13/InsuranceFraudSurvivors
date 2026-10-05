using UnityEngine;

[System.Serializable]
public class BulletPatternSettings : MonoBehaviour
{
    [Header("Base settings")]
    public int NumberOfBullets = 5;
    public float BulletSpeed = 10f;
    public float CooldownAfterShot;

    [Header("Offsets")]
    [Range(-1f, 1f)]
    public float PhaseOffset = 0f;
    [Range(-180f, 180f)]
    public float AngleOffset = 0f;

    [Header("Mask")]
    [Tooltip("Limita el patrón a cubrir cierto ángulo de la circunferencia")]
    public bool RadialMask;
    [Tooltip("Ángulo de la circunferencia que limita")]
    [Range(0f, 360f)]
    public float MaskAngle = 0f;
}
