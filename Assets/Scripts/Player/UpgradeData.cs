using UnityEngine;

/// <summary>
/// Scriptable object meant for upgrades the player can get.
/// </summary>
public abstract class UpgradeData : ScriptableObject
{
    public string id;            // Internal ID name
    public string title;         // Title for the Card
    public string description;   // Description for the Card

    [Header("Stackeo")]
    public int maxStacks = 1;    // How many of this Card the player can have during a run.
    [HideInInspector] 
    public int currentStacks = 0; // How many of this Card the player already has.

    public bool CanApply => currentStacks < maxStacks;

    /// <summary>
    /// Ensures the player can't get weapon upgrades if the player doesn't have the weapon.
    /// </summary>
    /// <param name="player">Player's Game Logic</param>
    /// <returns>Boolean. Defaults to True.</returns>
    public virtual bool IsAvailable(PlayerGameLogic player)
    {
        return true; // True by default, always available. Changed manually on the scriptable object.
    }

    public void ApplyStack(PlayerGameLogic player)
    {
        currentStacks++;
        Apply(player);
    }

    /// <summary>
    /// This function is called when the player chooses a Card.
    /// </summary>
    /// <param name="player">Player's Game Logic</param>
    public abstract void Apply(PlayerGameLogic player);
}
