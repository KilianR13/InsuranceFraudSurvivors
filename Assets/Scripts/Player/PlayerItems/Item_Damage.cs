using UnityEngine;

public class Item_Damage : Equippable
{
    [SerializeField] private float damageMultiplier = 0.1f;
    private float baseDamageMultiplier;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        baseDamageMultiplier = PlayerGlobalStats.Instance.DamageMultiplier;
        UpdateGlobalStats();
    }

    void UpdateGlobalStats()
    {
        PlayerGlobalStats.Instance.DamageMultiplier = baseDamageMultiplier + damageMultiplier;
    }

    protected override void ApplyUpgrade()
    {
        switch (currentLevel)
        {
            case 2:
                damageMultiplier += 0.1f; // 20% extra damage
                break;
            case 3:
                damageMultiplier += 0.1f; // 30% extra damage
                break;
            case 4:
                damageMultiplier += 0.2f; // 40% extra damage
                break;
            default:
                Debug.LogError("Wtf?");
                break;
        }
        UpdateGlobalStats();
    }
}
