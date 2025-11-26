using UnityEngine;

[CreateAssetMenu(fileName = "FireBallFireRate", menuName = "PlayerUpgrades/Fireball Firerate Up")]
public class FireBallFireRate  : UpgradeData
{

    public float FireBallFireRateDown = 0f;

    public override void Apply(PlayerGameLogic player)
    {
        player.FireBallFireRateReduction = FireBallFireRateDown;
        player.upgradeFireRateFireballs();
    }
}
