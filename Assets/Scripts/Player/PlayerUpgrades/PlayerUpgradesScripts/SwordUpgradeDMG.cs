using UnityEngine;

[CreateAssetMenu(fileName = "SwordUpgradeDMG", menuName = "PlayerUpgrades/Upgrade Sword DMG")]
public class SwordUpgradeDMG  : UpgradeData
{
    public int swordDamageIncrease;
    public GameObject swordPrefab;

    public override bool IsAvailable(PlayerGameLogic player)
    {
        // Only available if the player has the weapon.
        return player.weaponHandler.HasWeapon(swordPrefab);
    }

    public override void Apply(PlayerGameLogic player)
    {
        // player.swordUpgrade.SwordUpgradeDMG(swordDamageIncrease);
    }
}
