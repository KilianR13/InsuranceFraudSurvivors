using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

/// <summary>
/// Handles the logic of the player getting weapons
/// </summary>
public class PlayerWeaponHandler : MonoBehaviour
{
    public static PlayerWeaponHandler Instance { get; private set; }
    public List<GameObject> currentWeapons = new List<GameObject>();
    [Range(0, 6)]
    public int maxWeapons;
    [SerializeField] private GameObject StartingWeapon;
    [SerializeField] private Transform swordAttachPoint;

    
    void Start()
    {
        Instance = this;
        if (StartingWeapon.name == "sword") // Oh god this is AWWWWWWWWFUUUUUUUUUUULL KILL ME NOW
        {
            InstantiateSword(StartingWeapon);
        }
        else
        {
            InstantiateWeapon(StartingWeapon);
        }
    }

    public void AddWeapon(GameObject newWeapon)
    {
        if (currentWeapons.Count < maxWeapons)
        {
            currentWeapons.Add(newWeapon);
        }
    }

    public void InstantiateWeapon(GameObject newWeapon)
    {
        if (HasWeapon(newWeapon))
        {   
            Debug.LogError("Player tried to get weapon they already have");
            return;
        } 
        GameObject weapon = Instantiate(newWeapon, transform.position, Quaternion.identity);
        weapon.transform.SetParent(transform);
        AddWeapon(weapon);
    }

    public void InstantiateSword(GameObject newWeapon_Sword)
    {
        if (HasWeapon(newWeapon_Sword)) 
        {
            Debug.LogError("Player tried to get weapon (sword) they already have");
            return;
        }

        if (newWeapon_Sword == null || swordAttachPoint == null) return;

        GameObject currentSword = Instantiate(newWeapon_Sword);

        // The sword has an attach point, to prevent it being attached to the sword's "(0,0)", where we don't want it.
        Transform attachPoint = currentSword.transform.Find("AttachPoint");
        if (attachPoint == null)
        {
            return;
        }

        // We need to find the offset between the sword's "0,0,0" and it's attach point.
        Vector3 localOffset = currentSword.transform.InverseTransformPoint(attachPoint.position);

        // Setting the sword spawn point as the parent of the sword.
        currentSword.transform.SetParent(swordAttachPoint);

        // We put the sword using the offset.
        currentSword.transform.localPosition = -localOffset;
        currentSword.transform.localRotation = Quaternion.identity;

        Sword swordComp = currentSword.GetComponent<Sword>();
        if (swordComp != null)
        {
            // The sword needs a copy of the player's RigidBody so it can calculate the player's speed, and thus, damage multiplier.
            Rigidbody2D playerRb = GetComponentInParent<Rigidbody2D>();
            if (playerRb != null)
            {
                swordComp.SetPlayerRb(playerRb); 
            }  
        }
        AddWeapon(currentSword);
    }

    /// <summary>
    /// Checks if the asked-for weapon is in the list.
    /// </summary>
    /// <param name="weaponPrefab">Reference weapon to check the list</param>
    /// <returns>Boolean. If true, the weapon is in the list.</returns>
    public bool HasWeapon(GameObject weaponPrefab)
    {
        foreach (GameObject weapon in currentWeapons)
        {
            if (weapon.name == weaponPrefab.name + "(Clone)") // This is awful. But pray that it works...
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Obtains the GameObject stored in the list.
    /// </summary>
    /// <param name="weaponPrefab">Reference weapon</param>
    /// <returns>Weapon stored in the weapons list</returns>
    public GameObject GetWeapon(GameObject weaponPrefab)
    {
        foreach (GameObject weapon in currentWeapons)
        {
            if (weapon.name == weaponPrefab.name + "(Clone)") // This is awful. But pray that it works...
            {
                return weapon;
            }
        }
        return null;
    }

    public void UpgradeWeapon(GameObject upgradeableWeapon)
    {
        // var response = currentWeapons.Find(r => upgradeableWeapon);
    }
}
