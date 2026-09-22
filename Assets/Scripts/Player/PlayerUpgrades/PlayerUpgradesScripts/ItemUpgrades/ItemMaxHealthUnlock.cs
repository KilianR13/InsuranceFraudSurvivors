using UnityEngine;

[CreateAssetMenu(fileName = "ItemMaxHPUnlock", menuName = "PlayerUpgrades/Item_MaxHP Unlock")]
public class ItemMaxHPUnlock : UpgradeData
{
    public GameObject ItemMaxHPPrefab;

    public override bool IsAvailable(PlayerGameLogic player)
    {
        return !player.itemHandler.HasItem(ItemMaxHPPrefab) && !player.itemHandler.itemsListMaxxed;
    }
    public override void Apply(PlayerGameLogic player)
    {
        player.itemHandler.InstantiateItem(ItemMaxHPPrefab);
    }
}
