using UnityEngine;

[CreateAssetMenu(fileName = "SwordUpgrade", menuName = "PlayerUpgrades/Sword Upgrade")]
public class SwordSpawnUpgrade  : UpgradeData
{
    public override void Apply(PlayerGameLogic player)
    {
        player.swordUpgrade.SwordApplyUpgrade();
    }
}
