using UnityEngine;

[CreateAssetMenu(fileName = "ItemIncreasedRangeUpgrade", menuName = "PlayerUpgrades/Item_IncreasedRange Upgrade")]
public class ItemIncreasedRangeUpgrade : UpgradeData
{
    public GameObject itemIncreasedRangePrefab;

    public override bool IsAvailable(PlayerGameLogic player)
    {
        return !player.itemHandler.HasItem(itemIncreasedRangePrefab) && !player.itemHandler.itemsListMaxxed;
    }

    public override string GetDescription(PlayerGameLogic player)
    {
        GameObject itemObject = player.itemHandler.GetItem(itemIncreasedRangePrefab);

        if (itemObject == null)
            return description;

        Equippable item = itemObject.GetComponent<Equippable>();

        if (item == null)
            return description;

        return item.GetNextUpgradeDescription();
    }

    public override void Apply(PlayerGameLogic player)
    {
        player.itemHandler.UpgradeItem(itemIncreasedRangePrefab);
    }
}
