using UnityEngine;

[CreateAssetMenu(fileName = "SpeedLevelUp", menuName = "PlayerUpgrades/Item_Speed Level Up")]
public class Item_SpeedLevelUp  : UpgradeData
{
    public GameObject item_SpeedPrefab;

    public override bool IsAvailable(PlayerGameLogic player)
    {
        return player.itemHandler.HasItem(item_SpeedPrefab);
    }

    public override string GetDescription(PlayerGameLogic player)
    {
        GameObject itemObject = player.itemHandler.GetItem(item_SpeedPrefab);

        if (itemObject == null)
            return description;

        Equippable item = itemObject.GetComponent<Equippable>();

        if (item == null)
            return description;

        return item.GetNextUpgradeDescription();
    }

    public override void Apply(PlayerGameLogic player)
    {
        player.itemHandler.UpgradeItem(item_SpeedPrefab);
    }
}
