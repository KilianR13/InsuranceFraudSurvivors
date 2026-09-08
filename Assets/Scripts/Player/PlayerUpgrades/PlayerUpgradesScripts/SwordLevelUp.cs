using UnityEngine;

[CreateAssetMenu(fileName = "SwordLevelUp", menuName = "PlayerUpgrades/Sword Level Up")]
public class SwordLevelUp  : UpgradeData
{
    public GameObject swordPrefab;

    public override bool IsAvailable(PlayerGameLogic player)
    {
        // Only available if the player has the weapon.
        return player.weaponHandler.HasWeapon(swordPrefab);
    }

    public override string GetDescription(PlayerWeaponHandler weaponHandler)
    {
        GameObject swordObject = weaponHandler.GetWeapon(swordPrefab);

        if (swordObject == null)
            return description;

        Weapon weapon = swordObject.GetComponent<Weapon>();

        if (weapon == null)
            return description;

        return weapon.GetNextUpgradeDescription();
    }

    public override void Apply(PlayerGameLogic player)
    {
        player.weaponHandler.UpgradeWeapon(swordPrefab);
    }
}
