using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UpgradeDatabase", menuName = "PlayerUpgrades/Upgrade Database")]
public class UpgradeDatabase : ScriptableObject
{
    public List<UpgradeData> upgradeData = new List<UpgradeData>();
}
