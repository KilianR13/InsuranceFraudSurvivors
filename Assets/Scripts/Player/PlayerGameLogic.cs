using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerGameLogic : MonoBehaviour
{
    [Header("Health")]
    public int maxHealth;
    public GameObject healthBarGameoObject;
    private HealthBar healthBar;
    public Animator deathAnimation;
    private int currentHealth;
    public float healTimer;
    public int healAmmount;
    private int currentEXP;
    public int totalEXP;
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
    public GameObject firstButtonInPauseMenu; // Esto es un poco una estupidez pero sirve.
    private bool canPause;
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
        canPause = true;
        pauseMenu.SetActive(false);
        overLevelBonus = 0;
        playerMovement = GetComponent<PlayerMovement_Car>();
        currentLevel = 1;
        currentEXP = 0;
        totalEXP = 0;
        healTimer = 5f;
        healAmmount = 0;
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

            // Hacer la barra hija del jugador
            bar.transform.SetParent(transform);

            // Ajustar escala para evitar distorsión
            bar.transform.localScale = Vector3.one * 0.015f;

            // Obtener componente y configurar
            healthBar = bar.GetComponent<HealthBar>();
            if (healthBar != null)
            {
                healthBar.Initialize(transform, Camera.main); // Inicializa la barra de vida usando el transform del jugador y la cámara principal.
                healthBar.offset = new Vector3(0, 1.5f, 0);   // Posiciona la barra de vida encima del jugador.

                // Actualiza el valor inicial de vida
                healthBar.UpdateHealthbar(currentHealth, maxHealth);
            }
        }
        moneyEarned.text = $"$ = {totalEXP + overLevelBonus}";
        StartCoroutine(heal());
    }

    private IEnumerator heal()
    {
        yield return new WaitForSecondsRealtime(healTimer);
        if (currentHealth < maxHealth && healAmmount > 0) // Comprueba que el jugador no está a tope de vida y al menos puede curarse a sí mismo
        {
            if ((currentHealth + healAmmount) < maxHealth) // Comprueba que la cantidad de curación no lo pondría más allá del máximo de vida
            {
                currentHealth += healAmmount;    
            }
            else if ((currentHealth + healAmmount) >= maxHealth) // Este if quizás sea innecesario. Pone la vida al máximo.
            {
                currentHealth = maxHealth;
            }   
        }
        healthBar.UpdateHealthbar(currentHealth, maxHealth);
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
            playerMovement.engineSFX.Stop();
            GameManager.gm.gamePaused = true;
            GameManager.gm.dynamicMusic();
            canPause = false;
            OnLevelUp();
        }
    }
    
    private bool HasAvailableUpgrades()
    {
        return allUpgrades.Any(u => u.CanApply && u.IsAvailable(this));
    }


    private void OnLevelUp()
    {
        expBar.UpdateEXPBar(1,1);
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
        
        if (upgradePanel != null)
        {
            upgradePanel.SetActive(false);
        }
        pendingLevelUps--;
        if (pendingLevelUps > 0) // Todavía quedan niveles que subir, y por tanto, faltan mejoras que escoger.
        {
            // Vuelve a "reiniciar el sistema" como si recién estuviese subiendo de nivel.
            upgradeUIActive = false;
            Time.timeScale = 1f;
            CheckLevelUp();  // Vuelve a abrir las cartas
        }
        else // No quedan niveles que subir.
        {
            upgradeUIActive = false;
            expBar.StopRainbow();
            cardManager.ClearCards();
            expBar.UpdateEXPBar(currentEXP, expToNextLevel);
            playerMovement.engineSFX.Play();
            GameManager.gm.gamePaused = false;
            GameManager.gm.dynamicMusic();
            Time.timeScale = 1f;
            canPause = true;
        }
    }

    public void takeDamage(int damage)
    {
        currentHealth -= damage;
        
        if (currentHealth <= 0)
        {
            StopCoroutine(heal());
            healthBar.UpdateHealthbar(0, maxHealth);
            playerMovement.StopAllCoroutines();
            playerMovement.SilenceAllSound();
            deathAnimation.SetTrigger("PlayerDeath");
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
        if (canPause)
        {
            togglePauseMenu(); // Por alguna razón es necesario hacer una Corrutina porque si no, el juego se vuelve loco y llama mil veces al menú de pausa.    
        }
    }

    // Esto es estúpido pero yo lo soy más.
    // Si funciona, no se toca.
    private IEnumerator TogglePauseMenu_IENUM()
    {
        haveCalled = true;
        yield return new WaitForEndOfFrame();
        if (haveCalled)
        {
            pauseMenu.SetActive(!pauseMenu.activeSelf);
            Time.timeScale = pauseMenu.activeSelf? 0f : 1f;
            
            if (pauseMenu.activeSelf)
            {
                playerMovement.engineSFX.Stop();
                playerMovement.driftSFX.Stop();
                GameManager.gm.gamePaused = true;
                GameManager.gm.dynamicMusic();
            }
            else
            {
                GameManager.gm.gamePaused = false;
                GameManager.gm.dynamicMusic();
                playerMovement.engineSFX.Play();
            }

            // Seleccionar primer botón del menú para teclado/gamepad
            if (pauseMenu.activeSelf)
            {
                UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(firstButtonInPauseMenu);
            }
            haveCalled = false;
        }
        
    }

    public void togglePauseMenu()
    {
        StartCoroutine(TogglePauseMenu_IENUM());
    }

    public void restartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1f;
    }

    public void returnToMenu()
    {
        GameManager.gm.BackgroundMusicSFX.Stop();
        SceneManager.LoadScene("MainMenu");
    }
}
