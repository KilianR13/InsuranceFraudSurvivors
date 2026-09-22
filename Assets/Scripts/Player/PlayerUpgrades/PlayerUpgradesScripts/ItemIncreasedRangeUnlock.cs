using UnityEngine;

[CreateAssetMenu(fileName = "ItemIncreasedRangeUnlock", menuName = "PlayerUpgrades/Item_IncreasedRange Unlock")]
public class ItemIncreasedRangeUnlock : UpgradeData
{
    public GameObject itemIncreasedRangePrefab;

    public override bool IsAvailable(PlayerGameLogic player)
    {
        return !player.itemHandler.HasItem(itemIncreasedRangePrefab) && !player.itemHandler.itemsListMaxxed;
    }
    public override void Apply(PlayerGameLogic player)
    {
        player.itemHandler.InstantiateItem(itemIncreasedRangePrefab);
    }
}
