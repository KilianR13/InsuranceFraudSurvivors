using UnityEngine;

[CreateAssetMenu(fileName = "FireballUnlock", menuName = "PlayerUpgrades/Fireball Unlock")]
public class FireballUnlock  : UpgradeData
{
    public GameObject fireballInstancerPrefab;

    public override bool IsAvailable(PlayerGameLogic player)
    {
        // Only available if the player DOES NOT have the weapon and the slot is available.
        Equippable fireballItem = fireballInstancerPrefab.GetComponent<Equippable>();
        return !player.weaponHandler.HasWeapon(fireballInstancerPrefab) && !player.weaponHandler.IsSlotOccupied(fireballItem.itemSlot);
    }
    public override void Apply(PlayerGameLogic player)
    {
        player.weaponHandler.InstantiateWeapon(fireballInstancerPrefab);
    }
}
