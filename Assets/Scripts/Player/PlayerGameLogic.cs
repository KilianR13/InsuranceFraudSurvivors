using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PlayerGameLogic : MonoBehaviour
{
    public int maxHealth;
    private int currentHealth;
    private int currentEXP;
    private int currentLevel;

    [Header("Upgrades")]
    [SerializeField] public SwordUpgrade swordUpgrade;

    [Header("Available Upgrades")]
    public List<UpgradeData> allUpgrades = new List<UpgradeData>();


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
        {
            List<UpgradeData> selected = allUpgrades
                .OrderBy(x => Random.value)
                .Take(3)
                .ToList();

            cardManager.ShowCards(selected, OnCardSelected);
        }
        
        // Pausar el juego (física, timers, etc.)
        Time.timeScale = 0f;
    }

    private void OnCardSelected(UpgradeCard card)
    {
        Debug.Log($"Jugador eligió la mejora: {card.upgradeData.id}");
        ApplyUpgrade(card.upgradeData);

        // Aquí aplicas la mejora (más adelante). Por ahora solo cerramos.
        if (upgradePanel != null)
            upgradePanel.SetActive(false);
        expBar.StopRainbow();
        // Reanudar el juego
        Time.timeScale = 1f;
    }

    private void ApplyUpgrade(UpgradeData upgrade)
    {
        upgrade.Apply(this);
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
}
