using UnityEngine;

[CreateAssetMenu(fileName = "ItemDamageUnlock", menuName = "PlayerUpgrades/Item_Damage Unlock")]
public class ItemDamageUnlock  : UpgradeData
{
    public GameObject itemDamagePrefab;

    public override bool IsAvailable(PlayerGameLogic player)
    {
        return !player.itemHandler.HasItem(itemDamagePrefab) && !player.itemHandler.itemsListMaxxed;
    }
    
    public override void Apply(PlayerGameLogic player)
    {
        player.itemHandler.InstantiateItem(itemDamagePrefab);
    }
}
