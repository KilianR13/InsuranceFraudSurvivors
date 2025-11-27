using UnityEngine;

public abstract class UpgradeData : ScriptableObject
{
    public string id;            // ID interna
    public string title;         // Título que aparece en la carta
    public string description;   // Descripción en la carta

    [Header("Stackeo")]
    public int maxStacks = 1;    // <- máximo de veces que se puede elegir
    [HideInInspector] 
    public int currentStacks = 0; // <- cuántas veces lleva aplicada

    public bool CanApply => currentStacks < maxStacks;

    // Función que controla si la mejora puede aparecer. Es para evitar mejoras de armas que el jugador no tiene.
    public virtual bool IsAvailable(PlayerGameLogic player)
    {
        return true; // por defecto siempre disponible
    }

    public void ApplyStack(PlayerGameLogic player)
    {
        currentStacks++;
        Apply(player);
    }

    // Esto se ejecutará cuando el jugador elija la carta
    public abstract void Apply(PlayerGameLogic player);
}
