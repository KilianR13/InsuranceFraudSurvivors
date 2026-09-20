using UnityEngine;

public class Item_ExtraEXP : Equippable
{
    [SerializeField] private float EXPMultiplier = 0.1f;

    void Start()
    {
        UpdateGlobalStats();
    }

    void UpdateGlobalStats()
    {
        PlayerGlobalStats.Instance.EXPMultiplier += EXPMultiplier;
    }

    protected override void ApplyUpgrade()
    {
        switch (currentLevel)
        {
            case 1:
                EXPMultiplier += 0.05f; // 15%
                break;
            case 2:
                EXPMultiplier += 0.05f; // 20%
                break;
            case 3:
                EXPMultiplier += 0.05f; // 25%
                break;
            case 4:
                EXPMultiplier += 0.05f; // 30%
                break;
            case 5:
                EXPMultiplier += 0.1f;  // 40%
                break;
            default:
                Debug.LogError("Wtf?");
                break;
        }
        UpdateGlobalStats();
    }

}
