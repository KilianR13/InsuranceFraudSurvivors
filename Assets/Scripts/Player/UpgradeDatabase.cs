using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UpgradeDatabase", menuName = "PlayerUpgrades/Upgrade Database")]
public class UpgradeDatabase : ScriptableObject
{
    [Header("Weapons UpgradeData List")]
    public List<UpgradeData> weaponUpgrades = new List<UpgradeData>();

    [Header("Items UpgradeData List")]
    public List<UpgradeData> itemUpgrades = new List<UpgradeData>();

    // [HideInInspector]
    public List<UpgradeData> upgradeData = new List<UpgradeData>();

    private void OnEnable()
    {
        upgradeData.Clear();

        upgradeData.AddRange(weaponUpgrades);
        upgradeData.AddRange(itemUpgrades);
    }
}
