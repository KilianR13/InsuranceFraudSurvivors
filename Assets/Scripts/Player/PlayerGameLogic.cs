using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PlayerGameLogic : MonoBehaviour
{
    [Header("Health")]
    public int maxHealth;
    public GameObject healthBarGameoObject;
    private HealthBar healthBar;
    private int currentHealth;
    private int currentEXP;
    private int currentLevel;

    [Header("Upgrades")]
    [SerializeField] public SwordUpgrade swordUpgrade;

    [Header("Available Upgrades")]
    public List<UpgradeData> allUpgrades = new List<UpgradeData>();

    [Header("SFX")]
    [SerializeField] private AudioSource levelUp;
    


    [Header("Level System")]
    [SerializeField] private int baseEXPNeeded = 30;              // EXP necesaria para subir del nivel 1 al 2
    [SerializeField] private float expMultiplier = 1.5f;    // Cada nivel siguiente requiere más EXP
    [SerializeField] private EXPBar expBar;
    [SerializeField] private GameObject upgradePanel;          // el panel Canvas que contiene CardPanel (GameObject)
    [SerializeField] private UpgradeCardManager cardManager;    // componente que instancia cartas
    public int FireBallBonusDMG = 0;

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
        if (healthBarGameoObject != null)
        {
            GameObject bar = Instantiate(healthBarGameoObject);

            // Hacer la barra hija del enemigo
            bar.transform.SetParent(transform);

            // Ajustar escala para evitar distorsión
            bar.transform.localScale = Vector3.one * 0.015f;

            // Obtener componente y configurar
            healthBar = bar.GetComponent<HealthBar>();
            if (healthBar != null)
            {
                // Inicializar con target y la cámara principal
                Camera mainCam = Camera.main; // la cámara principal del jugador
                healthBar.Initialize(transform, mainCam);
                healthBar.offset = new Vector3(0, 1.5f, 0);   // súbelo 1 unidad


                // Actualizar el valor inicial de vida
                healthBar.UpdateHealthbar(currentHealth, maxHealth);
            }
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
        levelUp.Play();
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
        healthBar.UpdateHealthbar(currentHealth, maxHealth);
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
        currentHealth -= damage;
        
        if (currentHealth <= 0)
        {
            // GameManager.gm.loseGame();
            return;
        }
        if (healthBar != null)
            healthBar.UpdateHealthbar(currentHealth, maxHealth);
    }
}
