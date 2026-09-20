using UnityEngine;

public class Item_Damage : Equippable
{
    [SerializeField] private float damageMultiplier = 0.1f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UpdateGlobalStats();
    }

    void UpdateGlobalStats()
    {
        PlayerGlobalStats.Instance.DamageMultiplier += damageMultiplier;
    }

    protected override void ApplyUpgrade()
    {
        switch (currentLevel)
        {
            case 1:
                damageMultiplier += 0.1f;
                break;
            case 2:
                damageMultiplier += 0.1f;
                break;
            case 3:
                damageMultiplier += 0.2f;
                break;
            default:
                Debug.LogError("Wtf?");
                break;
        }
        UpdateGlobalStats();
    }
}
