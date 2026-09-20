using UnityEngine;

[CreateAssetMenu(fileName = "ItemSpeedUnlock", menuName = "PlayerUpgrades/Item_Speed Unlock")]
public class ItemSpeedUnlock  : UpgradeData
{
    public GameObject itemSpeedPrefab;

    public override bool IsAvailable(PlayerGameLogic player)
    {
        return !player.itemHandler.HasItem(itemSpeedPrefab) && !player.itemHandler.itemsListMaxxed;
    }

    public override void Apply(PlayerGameLogic player)
    {
        player.itemHandler.InstantiateItem(itemSpeedPrefab);
    }
}
