using UnityEngine;
using UnityEngine.UI;

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
    [SerializeField] private EXPBar expBar;
    [SerializeField] private GameObject upgradePanel;          // el panel Canvas que contiene CardPanel (GameObject)
    [SerializeField] private UpgradeCardManager cardManager;    // componente que instancia cartas

    private int expToNextLevel => Mathf.RoundToInt(baseEXPNeeded * Mathf.Pow(expMultiplier, currentLevel - 1));


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentLevel = 1;
        currentEXP = 0;
        expBar.UpdateEXPBar(currentEXP, expToNextLevel);
        expBar.UpdateLevel(currentLevel);
        currentHealth = maxHealth;
        if (upgradePanel != null)
        {
            upgradePanel.SetActive(false);    
        }
    }

    public void addEXP(int exp)
    {
        currentEXP += exp;
        swordUpgrade.TrySpawnSword(currentEXP);
        expBar.UpdateEXPBar(currentEXP, expToNextLevel);
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
        expBar.UpdateLevel(currentLevel);
        expBar.StartRainbow();
        if (upgradePanel != null)
        {
            upgradePanel.SetActive(true);    
        }
        

        if (cardManager != null)
            cardManager.ShowCards(3, OnCardSelected); // mostramos 3 cartas

        // Pausar el juego (física, timers, etc.)
        Time.timeScale = 0f;
        // Aquí luego podemos desbloquear mejoras, aumentar stats, etc.
    }

    private void OnCardSelected(UpgradeCard card)
    {
        Debug.Log($"Jugador eligió: {card.titleText.text}");

        // Aquí aplicas la mejora (más adelante). Por ahora solo cerramos.
        if (upgradePanel != null)
            upgradePanel.SetActive(false);
        expBar.StopRainbow();
        // Reanudar el juego
        Time.timeScale = 1f;
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
