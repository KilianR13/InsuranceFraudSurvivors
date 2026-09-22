using UnityEngine;

public class Item_Cooldown : Equippable
{
    [SerializeField] private float cooldownReduction = 0.08f;
    private float originalCooldownReduction;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        originalCooldownReduction = PlayerGlobalStats.Instance.CooldownMultiplier;
    }

    void UpdatePlayerStats()
    {
        PlayerGlobalStats.Instance.CooldownMultiplier = originalCooldownReduction - cooldownReduction;
    }

    protected override void ApplyUpgrade()
    {
        switch (currentLevel)
        {
            case 2:
                cooldownReduction -= 0.08f;  // 16% extra range
                break;
            case 3:
                cooldownReduction -= 0.08f;  // 24% extra range
                break;
            case 4:
                cooldownReduction -= 0.08f;  // 32% extra range
                break;
            case 5:
                cooldownReduction -= 0.08f;  // 40% extra range
                break;
            default:
                Debug.LogError("Wtf?");
                break;
        }
        UpdatePlayerStats();
    }
}
