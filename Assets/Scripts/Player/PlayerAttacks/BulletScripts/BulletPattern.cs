using UnityEngine;

public class BulletPattern : MonoBehaviour
{
    [Header("Base settings")]
    public int Repetitions;
    [Range(-180f, 180f)]
    public float AngleOffsetBetweenReps = 0f;
    public float StartWait = 0f;
    [Tooltip("Tiempo de espera después de que termine el patrón para que empiece el siguiente")]
    public float EndWait = 0f;
    [Tooltip("Configuración del patrón que tiene que seguir el ataque")]
    public BulletPatternSettings[] PatternSettings;
}
