using UnityEngine;

[CreateAssetMenu(fileName = "HealAmmount", menuName = "PlayerUpgrades/Heal Ammount")]
public class HealOverTimeAmmount  : UpgradeData
{
    public int HealAmmount;

    public override bool IsAvailable(PlayerGameLogic player)
    {
        // Always available.
        return true;
    }

    public override void Apply(PlayerGameLogic player)
    {
        player.healAmmount = HealAmmount;
    }
}
