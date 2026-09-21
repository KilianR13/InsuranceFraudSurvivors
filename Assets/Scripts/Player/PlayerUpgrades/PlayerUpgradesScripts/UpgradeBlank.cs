using UnityEngine;

[CreateAssetMenu(fileName = "DONOT", menuName = "PlayerUpgrades/DONT ITS EMPTY")]
public class UpgradeBlank : UpgradeData
{
    public GameObject prefab;

    public override bool IsAvailable(PlayerGameLogic player)
    {
        return false;
    }
    public override void Apply(PlayerGameLogic player)
    {
        
    }
}
