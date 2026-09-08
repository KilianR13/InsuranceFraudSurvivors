using UnityEngine;

[CreateAssetMenu(fileName = "FireballUnlock", menuName = "PlayerUpgrades/Fireball Unlock")]
public class FireballUnlock  : UpgradeData
{
    public GameObject fireballInstancerPrefab;

    public override bool IsAvailable(PlayerGameLogic player)
    {
        return !player.weaponHandler.HasWeapon(fireballInstancerPrefab) && !player.weaponHandler.weaponsListMaxxed;
    }
    public override void Apply(PlayerGameLogic player)
    {
        player.weaponHandler.InstantiateWeapon(fireballInstancerPrefab);
    }
}
