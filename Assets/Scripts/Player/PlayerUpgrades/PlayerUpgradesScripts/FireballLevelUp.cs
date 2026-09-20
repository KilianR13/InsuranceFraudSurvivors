using UnityEngine;

[CreateAssetMenu(fileName = "FireballLevelUp", menuName = "PlayerUpgrades/Fireball Level Up")]
public class FireballLevelUp  : UpgradeData
{
    public GameObject fireballInstancerPrefab;

    public override bool IsAvailable(PlayerGameLogic player)
    {
        return player.weaponHandler.HasWeapon(fireballInstancerPrefab);
    }

    public override string GetDescription(PlayerGameLogic player)
    {
        GameObject swordObject = player.weaponHandler.GetWeapon(fireballInstancerPrefab);

        if (swordObject == null)
            return description;

        Equippable weapon = swordObject.GetComponent<Equippable>();

        if (weapon == null)
            return description;

        return weapon.GetNextUpgradeDescription();
    }

    public override void Apply(PlayerGameLogic player)
    {
        player.weaponHandler.UpgradeWeapon(fireballInstancerPrefab);
    }
}
