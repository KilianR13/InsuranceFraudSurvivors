using UnityEngine;

[CreateAssetMenu(fileName = "ItemHealingUnlock", menuName = "PlayerUpgrades/Item_Heal Unlock")]
public class ItemHealingUnlock : UpgradeData
{
    public GameObject ItemHealingPrefab;

    public override bool IsAvailable(PlayerGameLogic player)
    {
        return !player.itemHandler.HasItem(ItemHealingPrefab) && !player.itemHandler.itemsListMaxxed;
    }
    public override void Apply(PlayerGameLogic player)
    {
        player.itemHandler.InstantiateItem(ItemHealingPrefab);
    }
}
