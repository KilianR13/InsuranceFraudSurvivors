using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [SerializeField] protected int currentLevel = 0;
    [SerializeField] protected int maxLevel = 8;
    public string[] upgradeDescriptions; // This is probably a bad practice...

    public int CurrentLevel => currentLevel;
    public int MaxLevel => maxLevel;

    public bool IsMaxLevel => currentLevel >= maxLevel;

    public void LevelUp()
    {
        if (IsMaxLevel)
            return;

        currentLevel++;

        ApplyUpgrade();
    }

    public string GetNextUpgradeDescription()
    {
        // if (currentLevel >= upgradeDescriptions.Length)
        //     return "MAX LEVEL";

        return upgradeDescriptions[currentLevel];
    }

    protected abstract void ApplyUpgrade();
}