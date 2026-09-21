using UnityEngine;

public class Item_IncreasedRange : Equippable
{
    [SerializeField] private float sizeMultiplier = 0.2f;
    private float originalSize;
    private Transform expBox;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        originalSize = PlayerGlobalStats.Instance.MaxHealthMultiplier;
        expBox = GetComponentInParent<PlayerGameLogic>().transform.Find("PlayerEXPBox");
        UpdatePlayerStats();
    }

    // Update is called once per frame
    void UpdatePlayerStats()
    {
        PlayerGlobalStats.Instance.EXPMagnetSize = originalSize + sizeMultiplier;
        expBox.localScale = Vector3.one * PlayerGlobalStats.Instance.EXPMagnetSize; // Will it work?
    }

    protected override void ApplyUpgrade()
    {
        switch (currentLevel)
        {
            case 1:
                sizeMultiplier += 0.1f;  // 30% extra range
                break;
            case 2:
                sizeMultiplier += 0.1f;  // 40% extra range
                break;
            case 3:
                sizeMultiplier += 0.1f;  // 50% extra range
                break;
            case 4:
                sizeMultiplier += 0.1f;  // 65% extra range
                break;
            case 5:
                sizeMultiplier += 0.2f;  // 80% extra range
                break;
            case 6:
                sizeMultiplier += 0.2f;  // 100% extra range
                break;
            default:
                Debug.LogError("Wtf?");
                break;
        }
        UpdatePlayerStats();
    }
}
