using UnityEngine;

[CreateAssetMenu(fileName = "FireBallFireRate", menuName = "PlayerUpgrades/Fireball Firerate Up")]
public class FireBallFireRate  : UpgradeData
{

    [Header("Upgrade")]
    public float FireBallFireRateDown = 0f;

    public override bool IsAvailable(PlayerGameLogic player)
    {
        // Only available if the player has the weapon.
        return true;
    }

    public override void Apply(PlayerGameLogic player)
    {
        // player.FireBallFireRateReduction = FireBallFireRateDown;
        
        // player.upgradeFireRateFireballs();
    }
}
