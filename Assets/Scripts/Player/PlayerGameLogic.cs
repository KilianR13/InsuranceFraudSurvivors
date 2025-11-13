using UnityEngine;

public class PlayerGameLogic : MonoBehaviour
{
    public int maxHealth;
    private int currentHealth;
    private int currentEXP;
    private int currentLevel;

    [Header("Upgrades")]
    [SerializeField] private SwordUpgrade swordUpgrade;

    [Header("Level System")]
    [SerializeField] private int baseEXPNeeded = 30;              // EXP necesaria para subir del nivel 1 al 2
    [SerializeField] private float expMultiplier = 1.5f;    // Cada nivel siguiente requiere más EXP

    private int expToNextLevel => Mathf.RoundToInt(baseEXPNeeded * Mathf.Pow(expMultiplier, currentLevel - 1));


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentLevel = 1;
        currentEXP = 0;
        currentHealth = maxHealth;
    }

    public void addEXP(int exp)
    {
        currentEXP += exp;
        swordUpgrade.TrySpawnSword(currentEXP);
        // Debug.Log($"Current EXP: {currentEXP}");
        CheckLevelUp();
    }

    private void CheckLevelUp()
    {
        while (currentEXP >= expToNextLevel)
        {
            currentEXP -= expToNextLevel;
            currentLevel++;
            OnLevelUp();
        }
    }

    private void OnLevelUp()
    {
        Debug.Log($"¡Subiste al nivel {currentLevel}!");
        // Aquí luego podemos desbloquear mejoras, aumentar stats, etc.
    }

    public void takeDamage(int damage)
    {
        if (currentHealth <= 0)
        {
            // GameManager.gm.loseGame();
            return;
        }
        currentHealth -= damage;
        // Debug.Log($"Current health: {currentHealth}");
    }

    /* ----------------------------------------------------------------------------------------------------- */
    // PREPARACIÓN PARA LA UI
    public float GetEXPProgress()
    {
        return (float)currentEXP / expToNextLevel;
    }

    public int GetCurrentLevel() => currentLevel;
    public int GetCurrentEXP() => currentEXP;
    public int GetEXPToNextLevel() => expToNextLevel;
}
