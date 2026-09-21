using UnityEngine;

[CreateAssetMenu(fileName = "ItemMaxHPUpgrade", menuName = "PlayerUpgrades/Item_MaxHP Upgrade")]
public class ItemMaxHPUpgrade : UpgradeData
{
    public GameObject ItemMaxHPPrefab;

    public override bool IsAvailable(PlayerGameLogic player)
    {
        return player.itemHandler.HasItem(ItemMaxHPPrefab);
    }

    public override string GetDescription(PlayerGameLogic player)
    {
        GameObject itemObject = player.itemHandler.GetItem(ItemMaxHPPrefab);

        if (itemObject == null)
            return description;

        Equippable item = itemObject.GetComponent<Equippable>();

        if (item == null)
            return description;

        return item.GetNextUpgradeDescription();
    }

    public override void Apply(PlayerGameLogic player)
    {
        player.itemHandler.UpgradeItem(ItemMaxHPPrefab);
    }
}
