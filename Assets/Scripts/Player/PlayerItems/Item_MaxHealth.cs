using UnityEngine;

public class Item_MaxHealth : Equippable
{
    [SerializeField] private float MaxHPMultipler = 0.1f;
    private float baseMaxHPMultiplier;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        baseMaxHPMultiplier = PlayerGlobalStats.Instance.MaxHealthMultiplier;
        UpdatePlayerStats();
    }

    void UpdatePlayerStats()
    {
        PlayerGlobalStats.Instance.MaxHealthMultiplier = baseMaxHPMultiplier + MaxHPMultipler;
    }

    protected override void ApplyUpgrade()
    {
        switch (currentLevel)
        {
            case 1:
                MaxHPMultipler += 0.05f; // 15% extra Max HP
                break;
            case 2:
                MaxHPMultipler += 0.05f; // 20% extra Max HP
                break;
            case 3:
                MaxHPMultipler += 0.1f;  // 30% extra Max HP
                break;
            case 4:
                MaxHPMultipler += 0.05f; // 35% extra Max HP
                break;
            case 5:
                MaxHPMultipler += 0.05f; // 40% extra Max HP
                break;
            case 6:
                MaxHPMultipler += 0.1f;  // 50% extra Max HP
                break;
            default:
                Debug.LogError("Wtf?");
                break;
        }
        UpdatePlayerStats();
    }
}
