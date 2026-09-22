using UnityEngine;

[CreateAssetMenu(fileName = "ItemHealingUpgrade", menuName = "PlayerUpgrades/Item_Heal Upgrade")]
public class ItemHealingUpgrade : UpgradeData
{
    public GameObject ItemHealingPrefab;

    public override bool IsAvailable(PlayerGameLogic player)
    {
        return player.itemHandler.HasItem(ItemHealingPrefab);
    }

    public override string GetDescription(PlayerGameLogic player)
    {
        GameObject itemObject = player.itemHandler.GetItem(ItemHealingPrefab);

        if (itemObject == null)
            return description;

        Equippable item = itemObject.GetComponent<Equippable>();

        if (item == null)
            return description;

        return item.GetNextUpgradeDescription();
    }

    public override void Apply(PlayerGameLogic player)
    {
        player.itemHandler.UpgradeItem(ItemHealingPrefab);
    }
}
