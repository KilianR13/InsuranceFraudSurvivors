using UnityEngine;

[CreateAssetMenu(fileName = "FireballDamageUpgradeData", menuName = "PlayerUpgrades/Fireball Damage Up")]
public class FireballDamageUpgradeData : UpgradeData
{
    public int bonusDamage = 5;

    public override void Apply(PlayerGameLogic player)
    {
        AutoAttackFireball autoFire = player.GetComponent<AutoAttackFireball>();
        if (autoFire != null)
        {
            autoFire.fireballPrefab.GetComponent<FireBall>().damage += bonusDamage;
        }
    }
}
