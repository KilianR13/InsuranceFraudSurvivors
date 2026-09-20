using UnityEngine;

[CreateAssetMenu(fileName = "ItemExtraEXPUnlock", menuName = "PlayerUpgrades/Item_ExtraEXP Unlock")]
public class ItemExtraEXPUnlock  : UpgradeData
{

    public GameObject itemExtraEXPPrefab;

    public override bool IsAvailable(PlayerGameLogic player)
    {
        return !player.itemHandler.HasItem(itemExtraEXPPrefab) && !player.itemHandler.itemsListMaxxed;
    }

    public override void Apply(PlayerGameLogic player)
    {
        player.itemHandler.InstantiateItem(itemExtraEXPPrefab);
    }
}
