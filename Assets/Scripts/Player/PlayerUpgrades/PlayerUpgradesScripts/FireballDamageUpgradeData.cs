using UnityEngine;

[CreateAssetMenu(fileName = "FireballDamageUpgradeData", menuName = "PlayerUpgrades/Fireball Damage Up")]
public class FireballDamageUpgradeData : UpgradeData
{
    public int bonusDamage = 5;

    public override bool IsAvailable(PlayerGameLogic player)
    {
        // Only appears if the player has the weapon.
        return true;
    }

    public override void Apply(PlayerGameLogic player)
    {
        // player.FireBallBonusDMG += bonusDamage;
    }
}
