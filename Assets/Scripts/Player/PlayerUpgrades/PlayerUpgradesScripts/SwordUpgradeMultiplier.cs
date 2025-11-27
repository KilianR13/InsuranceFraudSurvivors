using UnityEngine;

[CreateAssetMenu(fileName = "SwordUpgradeMultiplier", menuName = "PlayerUpgrades/Upgrade Sword Multiplier")]
public class SwordUpgradeMultiplier  : UpgradeData
{

    public float swordMultiplierIncrease;

    public override bool IsAvailable(PlayerGameLogic player)
    {
        // Solo aparece si el jugador NO tiene ya el arma
        return player.hasSword;
    }

    public override void Apply(PlayerGameLogic player)
    {
        player.swordUpgrade.SwordUpgradeMultiplier(swordMultiplierIncrease);
    }
}
