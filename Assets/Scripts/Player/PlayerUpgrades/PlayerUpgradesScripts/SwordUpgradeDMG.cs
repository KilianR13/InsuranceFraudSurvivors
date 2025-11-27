using UnityEngine;

[CreateAssetMenu(fileName = "SwordUpgradeDMG", menuName = "PlayerUpgrades/Upgrade Sword DMG")]
public class SwordUpgradeDMG  : UpgradeData
{

    public int swordDamageIncrease;

    public override bool IsAvailable(PlayerGameLogic player)
    {
        // Solo aparece si el jugador NO tiene ya el arma
        return player.hasSword;
    }

    public override void Apply(PlayerGameLogic player)
    {
        player.swordUpgrade.SwordUpgradeDMG(swordDamageIncrease);
    }
}
