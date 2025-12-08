using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerGameLogic : MonoBehaviour
{
    [Header("Health")]
    public int maxHealth;
    public GameObject healthBarGameoObject;
    private HealthBar healthBar;
    private int currentHealth;
    private int currentEXP;
    private int totalEXP;
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
    public GameObject pauseMenu;
    private bool haveCalled;
    

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
        pauseMenu.SetActive(false);
        overLevelBonus = 0;
        playerMovement = GetComponent<PlayerMovement_Car>();
        currentLevel = 1;
        currentEXP = 0;
        totalEXP = 0;
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
        totalEXP += exp;
        expBar.UpdateEXPBar(currentEXP, expToNextLevel);
        moneyEarned.text = $"$ = {totalEXP + overLevelBonus}";
        GameManager.gm.playerScore = totalEXP + overLevelBonus;
        CheckLevelUp();
    }

    private void CheckLevelUp()
    {
        while (currentEXP >= expToNextLevel)
        {
            currentEXP -= expToNextLevel;
            currentLevel++;
            GameManager.gm.playerLevel = currentLevel;
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

            // Actualiza la barra de EXP
            expBar.UpdateLevel(currentLevel);
            expBar.UpdateEXPBar(currentEXP, expToNextLevel);
            expBar.StopRainbow();

            // Limpia la UI
            upgradeUIActive = false;
            cardManager.ClearCards();
            if (upgradePanel != null)
            {
                upgradePanel.SetActive(false);
            }

            Time.timeScale = 1f;

            // Si hay más niveles que procesar, vuelve a comprobar el checklevelup.
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
            expBar.UpdateEXPBar(currentEXP, expToNextLevel);
            Time.timeScale = 1f;
        }
    }

    public void takeDamage(int damage)
    {
        currentHealth -= damage;
        
        if (currentHealth <= 0)
        {
            GameManager.gm.StageCompleted(false);
            return;
        }
        if (healthBar != null)
        {
            healthBar.UpdateHealthbar(currentHealth, maxHealth);
        }
    }

    public void OnCancel()
    {
        StartCoroutine(TogglePauseMenu());
    }

    // Esto es estúpido pero yo lo soy más.
    // Si funciona, no se toca.
    private IEnumerator TogglePauseMenu()
    {
        haveCalled = true;
        yield return new WaitForEndOfFrame();
        if (haveCalled)
        {
            pauseMenu.SetActive(!pauseMenu.activeSelf);
            Time.timeScale = pauseMenu.activeSelf? 0f : 1f;


            // Seleccionar primer botón del menú para teclado/gamepad
            // if (pauseMenu.activeSelf)
            // {
            //     UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(firstButtonInPauseMenu);
            // }
            Debug.Log($"Pausa {(pauseMenu.activeSelf ? "activada" : "desactivada")}");
            haveCalled = false;
        }
        
    }
}
