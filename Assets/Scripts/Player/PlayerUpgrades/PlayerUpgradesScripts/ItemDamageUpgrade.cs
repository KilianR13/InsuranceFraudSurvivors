using UnityEngine;

[CreateAssetMenu(fileName = "ItemDamageUpgrade", menuName = "PlayerUpgrades/Item_Damage Upgrade")]
public class ItemDamageUpgrade  : UpgradeData
{
    public GameObject itemDamagePrefab;

    public override bool IsAvailable(PlayerGameLogic player)
    {
        return player.itemHandler.HasItem(itemDamagePrefab);
    }

    public override string GetDescription(PlayerGameLogic player)
    {
        GameObject itemObject = player.itemHandler.GetItem(itemDamagePrefab);

        if (itemObject == null)
            return description;

        Equippable item = itemObject.GetComponent<Equippable>();

        if (item == null)
            return description;

        return item.GetNextUpgradeDescription();
    }

    public override void Apply(PlayerGameLogic player)
    {
        player.itemHandler.UpgradeItem(itemDamagePrefab);
    }
}
