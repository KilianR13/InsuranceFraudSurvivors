using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
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
    public bool hasSword = false;

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
    private int pendingLevelUps = 0;
    private bool upgradeUIActive = false;

    public int FireBallBonusDMG = 0;
    public float FireBallFireRateReduction = 0f;

    [Header("UI")]
    public TextMeshProUGUI moneyEarned;
    public int overLevelBonus = 0;
    

    public event Action OnSignal;
    private PlayerMovement_Car playerMovement;

    private int expToNextLevel => Mathf.RoundToInt(baseEXPNeeded * Mathf.Pow(expMultiplier, currentLevel - 1));

    public void upgradeFireRateFireballs()
    {
        OnSignal?.Invoke();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        overLevelBonus = 0;
        playerMovement = GetComponent<PlayerMovement_Car>();
        currentLevel = 1;
        currentEXP = 0;
        hasSword = false;
        
        foreach (var u in allUpgrades)
        {
            u.currentStacks = 0;
        }
        
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
        moneyEarned.text = $"$ = {currentEXP + overLevelBonus}";
        CheckLevelUp();
    }

    private void CheckLevelUp()
    {
        while (currentEXP >= expToNextLevel)
        {
            currentEXP -= expToNextLevel;
            currentLevel++;
            pendingLevelUps++;   // ← Guardamos que hay que mostrar otra mejora
        }

        // Si no hay un panel activo, empezar la cadena de mejoras
        if (!upgradeUIActive && pendingLevelUps > 0)
        {
            OnLevelUp();
        }
    }
    
    private bool HasAvailableUpgrades()
    {
        return allUpgrades.Any(u => u.CanApply && u.IsAvailable(this));
    }


    private void OnLevelUp()
    {
        if (!HasAvailableUpgrades())
        {
            overLevelBonus += 50;
            pendingLevelUps--;

            // ACTUALIZAR BARRA DE EXP CORRECTAMENTE
            expBar.UpdateLevel(currentLevel);
            expBar.UpdateEXPBar(currentEXP, expToNextLevel);
            expBar.StopRainbow();

            // LIMPIAR UI
            upgradeUIActive = false;
            cardManager.ClearCards();
            if (upgradePanel != null)
                upgradePanel.SetActive(false);

            Time.timeScale = 1f;

            // SEGUIR PROCESANDO NIVELES SI QUEDAN MÁS POR AÑADIR
            if (pendingLevelUps > 0)
            {
                CheckLevelUp();
            }

            return;
        }



        upgradeUIActive = true;
        playerMovement.driftSFX.Stop();

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
                .Where(u => u.CanApply) // Filtra las mejoras dependiendo de si el jugador puede tener más copias de la misma mejora.
                .Where(u => u.IsAvailable(this)) // Filtra las mejoras dependiendo de si están disponibles (Si el jugador tiene el arma o no).
                .OrderBy(x => UnityEngine.Random.value)
                .Take(3)
                .ToList();

            cardManager.ShowCards(selected, OnCardSelected);
        }
        // Pausar el juego (física, timers, etc.)
        Time.timeScale = 0f;
    }

    private void OnCardSelected(UpgradeCard card)
    {
        UpgradeData upgrade = card.upgradeData;

        Debug.Log($"Jugador eligió la mejora: {upgrade.id}");

        // Registra el stack y aplica la mejora. 
        upgrade.ApplyStack(this);
        
        healthBar.UpdateHealthbar(currentHealth, maxHealth);
        
        // Aquí aplicas la mejora (más adelante). Por ahora solo cerramos.
        if (upgradePanel != null)
        {
            upgradePanel.SetActive(false);
        }
        pendingLevelUps--;
        if (pendingLevelUps > 0)
        {
            // Todavía quedan mejoras, pero **NO** llames directamente a OnLevelUp()
            // Solo prepara el estado para que CheckLevelUp() las invoque correctamente.
            upgradeUIActive = false;
            Time.timeScale = 1f;
            CheckLevelUp();  // ← esto volverá a abrir las cartas SIN romper nada
        }
        else
        {
            upgradeUIActive = false;
            expBar.StopRainbow();
            cardManager.ClearCards();
            Time.timeScale = 1f;
        }
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
