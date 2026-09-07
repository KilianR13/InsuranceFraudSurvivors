using UnityEngine;

[CreateAssetMenu(fileName = "SwordUpgradeMultiplier", menuName = "PlayerUpgrades/Upgrade Sword Multiplier")]
public class SwordUpgradeMultiplier  : UpgradeData
{
    public float swordMultiplierIncrease;
    public GameObject swordPrefab;

    public override bool IsAvailable(PlayerGameLogic player)
    {
        // Only available if the player has the weapon.
        return player.weaponHandler.HasWeapon(swordPrefab);
    }

    public override void Apply(PlayerGameLogic player)
    {
        // player.swordUpgrade.SwordUpgradeMultiplier(swordMultiplierIncrease);
    }
}
