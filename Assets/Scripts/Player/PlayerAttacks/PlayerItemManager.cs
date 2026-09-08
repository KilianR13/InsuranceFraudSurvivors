using System.Collections.Generic;
using UnityEngine;

public class PlayerItemHandler : MonoBehaviour
{
    public static PlayerItemHandler Instance { get; private set; }
    public List<GameObject> currentItems = new List<GameObject>();
    [Range(1, 6)]
    public int maxItems;
    public bool itemsListMaxxed;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Instance = this;
    }

    public void AddItem(GameObject newItem)
    {
        if (itemsListMaxxed) return;

        if (currentItems.Count < maxItems)
        {
            currentItems.Add(newItem);
            if (currentItems.Count == maxItems)
            {
                itemsListMaxxed = true;
            }
        }
    }

    /// <summary>
    /// Checks if the asked-for weapon is in the list.
    /// </summary>
    /// <param name="itemPrefab">Reference item to check the list</param>
    /// <returns>Boolean. If true, the item is in the list.</returns>
    public bool HasItem(GameObject itemPrefab)
    {
        foreach (GameObject item in currentItems)
        {
            if (item.name == itemPrefab.name + "(Clone)") // This is awful. But pray that it works...
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Obtains the GameObject stored in the list.
    /// </summary>
    /// <param name="itemPrefab">Reference item</param>
    /// <returns>Item stored in the passive items list</returns>
    public GameObject GetItem(GameObject itemPrefab)
    {
        foreach (GameObject item in currentItems)
        {
            if (item.name == itemPrefab.name + "(Clone)") // This is awful. But pray that it works...
            {
                return item;
            }
        }
        return null;
    }

    public void UpgradeItem(GameObject upgradeableItem)
    {
        GameObject itemGO = GetItem(upgradeableItem);
        if (itemGO == null) return;

        Equippable item = itemGO.GetComponent<Equippable>();
        if (item == null) return;

        item.LevelUp();
    }
}
