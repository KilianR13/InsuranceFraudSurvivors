using UnityEngine;

[CreateAssetMenu(fileName = "HealOverTimeTimer", menuName = "PlayerUpgrades/Healing Timer Down")]
public class HealOverTimeTimer  : UpgradeData
{
    public float HealTimerDown;

    public override bool IsAvailable(PlayerGameLogic player)
    {
        // Siempre está disponible
        return true;
    }

    public override void Apply(PlayerGameLogic player)
    {
        player.healTimer -= HealTimerDown;
    }
}
