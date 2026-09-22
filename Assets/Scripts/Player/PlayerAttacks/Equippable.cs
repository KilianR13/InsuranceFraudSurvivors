using UnityEngine;

public enum ItemSlotType
{
    Front,
    Back,
    Side,
    Top,
    Special,
    PassiveItem
}

public abstract class Equippable : MonoBehaviour
{
    [SerializeField] protected int currentLevel = 1;
    [SerializeField] protected int maxLevel = 8;
    public ItemSlotType itemSlot;
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

        return upgradeDescriptions[currentLevel - 1];
    }

    protected abstract void ApplyUpgrade();
}