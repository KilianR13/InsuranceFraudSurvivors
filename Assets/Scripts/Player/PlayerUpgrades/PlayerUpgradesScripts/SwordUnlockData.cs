using UnityEngine;

[CreateAssetMenu(fileName = "SwordUnlock", menuName = "PlayerUpgrades/Sword Upgrade")]
public class SwordUnlockUpgrade  : UpgradeData
{

    public override bool IsAvailable(PlayerGameLogic player)
    {
        // Only available if the player DOES NOT have the weapon.
        return !player.hasSword;
    }

    public override void Apply(PlayerGameLogic player)
    {
        player.swordUpgrade.SpawnSword();
        player.hasSword = true;
    }
}
