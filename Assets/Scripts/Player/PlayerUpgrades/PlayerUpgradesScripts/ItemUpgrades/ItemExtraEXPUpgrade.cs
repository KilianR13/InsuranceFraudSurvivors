using UnityEngine;

[CreateAssetMenu(fileName = "ItemExtraEXPUpgrade", menuName = "PlayerUpgrades/Item_ExtraEXP Upgrade")]
public class ItemExtraEXPUpgrade  : UpgradeData
{

    public GameObject itemExtraEXPPrefab;

    public override bool IsAvailable(PlayerGameLogic player)
    {
        return player.itemHandler.HasItem(itemExtraEXPPrefab);
    }

    public override string GetDescription(PlayerGameLogic player)
    {
        GameObject itemObject = player.itemHandler.GetItem(itemExtraEXPPrefab);

        if (itemObject == null)
            return description;

        Equippable item = itemObject.GetComponent<Equippable>();

        if (item == null)
            return description;

        return item.GetNextUpgradeDescription();
    }

    public override void Apply(PlayerGameLogic player)
    {
        player.itemHandler.UpgradeItem(itemExtraEXPPrefab);
    }
}
