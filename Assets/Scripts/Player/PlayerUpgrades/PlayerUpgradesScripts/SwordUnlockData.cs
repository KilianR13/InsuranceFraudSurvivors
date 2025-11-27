using UnityEngine;

[CreateAssetMenu(fileName = "SwordUnlock", menuName = "PlayerUpgrades/Sword Upgrade")]
public class SwordUnlockUpgrade  : UpgradeData
{

    public override bool IsAvailable(PlayerGameLogic player)
    {
        // Solo aparece si el jugador NO tiene ya el arma
        return !player.hasSword;
    }

    public override void Apply(PlayerGameLogic player)
    {
        player.swordUpgrade.SpawnSword();
        player.hasSword = true;
    }
}
