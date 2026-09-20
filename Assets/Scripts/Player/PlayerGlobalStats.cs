using UnityEngine;

public class PlayerGlobalStats : MonoBehaviour
{
    public static PlayerGlobalStats Instance {get; private set;}

    [Range(0.1f, 1f)]
    public float CooldownMultiplier = 1f;
    [Range(1f, 100f)]
    public float DamageMultiplier = 1f;
    [Range(1f, 100f)]
    public float MaxHealthMultiplier = 1f;
    [Range(1f, 5f)]
    public float AccelMultiplier = 1f;
    [Range(1f, 5f)]
    public float MaxSpeedMultiplier = 1f;
    [Range(1f, 5f)]
    public float EXPMultiplier = 1f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        Instance = this;
        CooldownMultiplier = 1f - PermanentUpgradeManager.Instance.data.Permanent_CooldownReduction;
        DamageMultiplier = 1f * PermanentUpgradeManager.Instance.data.Permanent_DamageMultiplier;
        MaxHealthMultiplier = 1f * PermanentUpgradeManager.Instance.data.Permanent_MaxHealthMultiplier;
        AccelMultiplier = 1f * PermanentUpgradeManager.Instance.data.Permanent_AccelMultiplier;
        MaxSpeedMultiplier = 1f * PermanentUpgradeManager.Instance.data.Permanent_MaxSpeedMultiplier;
        EXPMultiplier = 1f * PermanentUpgradeManager.Instance.data.Permanent_EXPMultiplier;
    }

}
