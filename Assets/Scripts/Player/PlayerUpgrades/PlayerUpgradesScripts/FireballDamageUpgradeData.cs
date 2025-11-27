using UnityEngine;

[CreateAssetMenu(fileName = "FireballDamageUpgradeData", menuName = "PlayerUpgrades/Fireball Damage Up")]
public class FireballDamageUpgradeData : UpgradeData
{
    public int bonusDamage = 5;

    public override bool IsAvailable(PlayerGameLogic player)
    {
        // Solo aparece si el jugador NO tiene ya el arma
        return true;
    }

    public override void Apply(PlayerGameLogic player)
    {
        player.FireBallBonusDMG += bonusDamage;
    }
}
