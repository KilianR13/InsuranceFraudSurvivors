using UnityEngine;

[CreateAssetMenu(fileName = "MaxHealthUp", menuName = "PlayerUpgrades/Max Health Up")]
public class MaxHealthUpgrade : UpgradeData
{
    public int healthIncrease = 20;

    public override void Apply(PlayerGameLogic player)
    {
        player.maxHealth += healthIncrease;
    }
}
