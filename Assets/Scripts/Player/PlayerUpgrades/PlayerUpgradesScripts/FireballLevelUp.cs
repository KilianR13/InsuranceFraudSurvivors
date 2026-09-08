using UnityEngine;

[CreateAssetMenu(fileName = "FireballLevelUp", menuName = "PlayerUpgrades/Fireball Level Up")]
public class FireballLevelUp  : UpgradeData
{
    public GameObject fireballInstancerPrefab;

    public override bool IsAvailable(PlayerGameLogic player)
    {
        return player.weaponHandler.HasWeapon(fireballInstancerPrefab);
    }

    public override string GetDescription(PlayerWeaponHandler weaponHandler)
    {
        GameObject swordObject = weaponHandler.GetWeapon(fireballInstancerPrefab);

        if (swordObject == null)
            return description;

        Weapon weapon = swordObject.GetComponent<Weapon>();

        if (weapon == null)
            return description;

        return weapon.GetNextUpgradeDescription();
    }

    public override void Apply(PlayerGameLogic player)
    {
        player.weaponHandler.UpgradeWeapon(fireballInstancerPrefab);
    }
}
