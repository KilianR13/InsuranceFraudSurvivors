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
            case 1:
                accelMultiplier += 0.1f;
                break;
            case 2:
                maxSpeedMultiplier += 0.1f;
                break;
            case 3:
                accelMultiplier += 0.2f;
                break;
            case 4:
                maxSpeedMultiplier += 0.2f;
                break;
            case 5:
                maxSpeedMultiplier += 0.3f;
                break;
            case 6:
                accelMultiplier += 0.2f;
                break;
            case 7:
                accelMultiplier += 0.3f;
                break;
            case 8:
                maxSpeedMultiplier += 0.3f;
                break;
            default:
                Debug.LogError("Wtf?");
                break;
        }
        UpdatePlayerMovement();
    }
}
