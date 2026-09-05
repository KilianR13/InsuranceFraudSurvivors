using UnityEngine;

/// <summary>
/// Class that handles the process of obtaining and upgrading the Sword weapon.
/// </summary>
public class SwordUpgrade : MonoBehaviour
{
    [Header("Prefabs y referencias")]
    [SerializeField] private GameObject swordPrefab;        // The original prefab of the Sword.
    [SerializeField] private Transform swordSpawnPoint;     // An attachment point in the player. Normally empty.

    private GameObject currentSword;    // GameObject containing a reference to the sword. Only not null when the player has the weapon.

    /// <summary>
    /// Upgrades the damage of the sword.
    /// </summary>
    /// <param name="upgradeDamage">How much damage is added</param>
    public void SwordUpgradeDMG(int upgradeDamage)
    {
        // Ensures the sword exists.
        Sword swordComp = currentSword.GetComponent<Sword>();
        if (swordComp != null)
        {
            swordComp.baseDamage += upgradeDamage;
        }
    }

    /// <summary>
    /// Upgrades the speed multiplier of the sword.
    /// </summary>
    /// <param name="upgradeMultiplier">How much the multiplier increases</param>
    public void SwordUpgradeMultiplier(float upgradeMultiplier)
    {
        // Si ya existe, aumentamos su daño
        Sword swordComp = currentSword.GetComponent<Sword>();
        if (swordComp != null)
        {
            swordComp.damageMultiplier += upgradeMultiplier;
        }
    }

    /// <summary>
    /// Function to add the Sword to the player's weapons.
    /// </summary>
    public void SpawnSword()
    {
        if (swordPrefab == null || swordSpawnPoint == null) return;

        currentSword = Instantiate(swordPrefab);

        // The sword has an attach point, to prevent it being attached to the sword's "(0,0)", where we don't want it.
        Transform attachPoint = currentSword.transform.Find("AttachPoint");
        if (attachPoint == null)
        {
            return;
        }

        // We need to find the offset between the sword's "0,0,0" and it's attach point.
        Vector3 localOffset = currentSword.transform.InverseTransformPoint(attachPoint.position);

        // Setting the sword spawn point as the parent of the sword.
        currentSword.transform.SetParent(swordSpawnPoint);

        // We put the sword using the offset.
        currentSword.transform.localPosition = -localOffset;
        currentSword.transform.localRotation = Quaternion.identity;

        Sword swordComp = currentSword.GetComponent<Sword>();
        if (swordComp != null)
        {
            // The sword needs a copy of the player's RigidBody so it can calculate the player's speed, and thus, damage multiplier.
            Rigidbody2D playerRb = swordSpawnPoint.GetComponentInParent<Rigidbody2D>();
            if (playerRb != null)
            {
                swordComp.SetPlayerRb(playerRb); 
            }
                
        }

    }
}
