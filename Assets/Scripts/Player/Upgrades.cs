using UnityEngine;

public abstract class UpgradeData : ScriptableObject
{
    public string id;            // ID interna (ej: "SWORD", "FIREBALL_DAMAGE", etc.)
    public string title;         // Título que aparece en la carta
    public string description;   // Descripción en la carta

    // Esto se ejecutará cuando el jugador elija la carta
    public abstract void Apply(PlayerGameLogic player);
}
