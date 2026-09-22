using UnityEngine;

public class Item_Speed : Equippable
{
    [SerializeField] private float accelMultiplier = 0.2f;

    [SerializeField] private float maxSpeedMultiplier = 0.1f;
    private float baseAccelMultiplier;
    private float baseMaxSpeedMultiplier;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        baseAccelMultiplier = PlayerGlobalStats.Instance.AccelMultiplier;
        baseMaxSpeedMultiplier = PlayerGlobalStats.Instance.MaxSpeedMultiplier;
        UpdatePlayerMovement();
    }

    void UpdatePlayerMovement()
    {   
        PlayerGlobalStats.Instance.AccelMultiplier = baseAccelMultiplier + accelMultiplier;
        PlayerGlobalStats.Instance.MaxSpeedMultiplier = baseMaxSpeedMultiplier + maxSpeedMultiplier;
    }

    protected override void ApplyUpgrade()
    {
        switch (currentLevel)
        {
            case 2:
                accelMultiplier += 0.1f;
                break;
            case 3:
                maxSpeedMultiplier += 0.1f;
                break;
            case 4:
                accelMultiplier += 0.2f;
                break;
            case 5:
                maxSpeedMultiplier += 0.2f;
                break;
            case 6:
                maxSpeedMultiplier += 0.3f;
                break;
            case 7:
                accelMultiplier += 0.2f;
                break;
            case 8:
                accelMultiplier += 0.3f;
                break;
            case 9:
                maxSpeedMultiplier += 0.3f;
                break;
            default:
                Debug.LogError("Wtf?");
                break;
        }
        UpdatePlayerMovement();
    }
}
