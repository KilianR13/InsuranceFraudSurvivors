using UnityEngine;

[CreateAssetMenu(fileName = "SwordUnlock", menuName = "PlayerUpgrades/Sword Upgrade")]
public class SwordUnlockUpgrade  : UpgradeData
{
    public GameObject swordPrefab;

    public override bool IsAvailable(PlayerGameLogic player)
    {
        // Only available if the player DOES NOT have the weapon.
        return !player.weaponHandler.HasWeapon(swordPrefab);
    }

    public override void Apply(PlayerGameLogic player)
    {
        player.weaponHandler.InstantiateSword(swordPrefab);
    }
}
