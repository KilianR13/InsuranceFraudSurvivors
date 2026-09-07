using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Handles extensive logic about the player.
/// </summary>
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
    public PlayerWeaponHandler weaponHandler;

    [Header("Available Upgrades")]
    public List<UpgradeData> allUpgrades = new List<UpgradeData>();

    [Header("SFX")]
    [SerializeField] private AudioSource levelUp;
    


    [Header("Level System")]
    [SerializeField] private int baseEXPNeeded = 30;            // Base EXP required to go from level 1 to level 2.
    [SerializeField] private float expMultiplier = 1.5f;        // Multiplier to make following levels require exponentially more EXP.
    [SerializeField] private EXPBar expBar;
    [SerializeField] private GameObject upgradePanel;           // The Canvas that will contain the GameObject with "CardPanel"
    [SerializeField] private UpgradeCardManager cardManager;    // Card Manager that will instatiate the Upgrade Cards.
    private int pendingLevelUps = 0;
    private bool upgradeUIActive = false;


    [Header("UI")]
    public TextMeshProUGUI moneyEarned;
    public int overLevelBonus = 0;
    public GameObject pauseMenu;
    public GameObject firstButtonInPauseMenu; // Controller purposes.
    private bool canPause;
    private bool haveCalled;
    

    // public event Action OnSignal;
    private PlayerMovement_Car playerMovement;

    private int expToNextLevel => Mathf.RoundToInt(baseEXPNeeded * Mathf.Pow(expMultiplier, currentLevel - 1));

    
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

            // Makes the healthbar a child of the player.
            bar.transform.SetParent(transform);

            // Required to make the healthbar look nice.
            bar.transform.localScale = Vector3.one * 0.015f;

            // Configures the healthbar
            healthBar = bar.GetComponent<HealthBar>();
            if (healthBar != null)
            {
                healthBar.Initialize(transform, Camera.main);   // Uses the player's transform and camera to initialize the healthbar.
                healthBar.offset = new Vector3(0, 1.5f, 0);     // Positions the healthbar slightly above the player.

                // Updates the healthbar.
                healthBar.UpdateHealthbar(currentHealth, maxHealth);
            }
        }
        if (weaponHandler == null)
        {
            weaponHandler = GetComponentInChildren<PlayerWeaponHandler>();    
        }
        moneyEarned.text = $"$ = {totalEXP + overLevelBonus}"; // Debug.
        StartCoroutine(heal());
    }

    /// <summary>
    /// Function that heals the player. Runs on forever until the player dies or finishes the game.
    /// </summary>
    /// <returns></returns>
    private IEnumerator heal()
    {
        while (true) {
        yield return new WaitForSecondsRealtime(healTimer);
        if (currentHealth < maxHealth && healAmmount > 0)   // Checks if the player can heal themselves or needs to.
        {
            if ((currentHealth + healAmmount) < maxHealth)
            {
                currentHealth += healAmmount;    
            }
            else
            {
                currentHealth = maxHealth;
            }   
        }
        healthBar.UpdateHealthbar(currentHealth, maxHealth);
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
            pendingLevelUps++;   // Like this we can store multiple level ups in a row!
        }

        // If the panel is not currently active and there's at least 1 level up pending, starts the level up chain.
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

    /// <summary>
    /// Function called when the player levels up. Handles many, many things.
    /// </summary>
    private void OnLevelUp()
    {
        expBar.UpdateEXPBar(1,1);
        if (!HasAvailableUpgrades())
        {
            overLevelBonus += 50; // Extra money for the player to spend on upgrades!
            pendingLevelUps--;

            // Updates the EXP bar.
            expBar.UpdateLevel(currentLevel);
            expBar.UpdateEXPBar(currentEXP, expToNextLevel);
            expBar.StopRainbow();

            // Removes all the upgrade UI from the screen.
            upgradeUIActive = false;
            cardManager.ClearCards();
            if (upgradePanel != null)
            {
                upgradePanel.SetActive(false);
            }

            Time.timeScale = 1f;

            // If there are more pending levels, checks level up to begin the process once again.
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
                .Where(u => u.CanApply)                 // Filters the upgrades deppending if the player can get more upgrades for the weapon.
                .Where(u => u.IsAvailable(this))        // Filters deppending if the player has a weapon, and thus, can upgrade it.
                .OrderBy(x => UnityEngine.Random.value) // Randomizes the order of the available upgrades.
                .Take(3)                                // Takes 3 of them.
                .ToList();                              // Turns them into a list.

            cardManager.ShowCards(selected, OnCardSelected);
        }
        // Pauses the game while the player selects a card.
        Time.timeScale = 0f;
    }

    private void OnCardSelected(UpgradeCard card)
    {
        UpgradeData upgrade = card.upgradeData;

        // Registers the stack and applies the upgrade.
        upgrade.ApplyStack(this);
        
        healthBar.UpdateHealthbar(currentHealth, maxHealth);
        
        if (upgradePanel != null)
        {
            upgradePanel.SetActive(false);
        }
        pendingLevelUps--;

        if (pendingLevelUps > 0) // There are 1 or more level ups pending.
        {
            // "Restarts" the upgrade system, so to speak.
            upgradeUIActive = false;
            Time.timeScale = 1f;
            CheckLevelUp();  // Reopens the chance to pick an upgrade.
        }
        else // There are no more level ups pending.
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

    public void AddWeapon()
    {
        
    }

    public void ApplyUpgrade()
    {
        // weaponHandler.UpgradeWeapon
    }

    /// <summary>
    /// Handles the player taking damage.
    /// </summary>
    /// <param name="damage">Ammount of damage the player is taking</param>
    public void takeDamage(int damage)
    {
        currentHealth -= damage;
        
        if (currentHealth <= 0)
        {
            PlayerDeath();
            return;
        }
        if (healthBar != null)
        {
            healthBar.UpdateHealthbar(currentHealth, maxHealth);
        }
    }

    /// <summary>
    /// Function that handles the player's death.
    /// </summary>
    private void PlayerDeath()
    {
        StopCoroutine(heal());
        healthBar.UpdateHealthbar(0, maxHealth);
        playerMovement.StopAllCoroutines();
        playerMovement.SilenceAllSound();
        deathAnimation.SetTrigger("PlayerDeath");
        GameManager.gm.StageCompleted(false);
    }

    public void OnCancel()
    {
        if (canPause)
        {
            togglePauseMenu(); // Yes, a coroutine is needed here.
            // StartCoroutine(TogglePauseMenu_IENUM()); CAN'T JUST DO THIS?
        }
    }

    /// <summary>
    /// Incredibly stupid coroutine. Handles the pause menu.
    /// </summary>
    /// <returns></returns>
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

            // Selects the first button in the pause menu for gamepad purposes.
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
        playerMovement.StopAllCoroutines();
        playerMovement.SilenceAllSound();
        StopAllCoroutines();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1f;
    }

    public void returnToMenu()
    {
        GameManager.gm.BackgroundMusicSFX.Stop();
        playerMovement.StopAllCoroutines();
        playerMovement.SilenceAllSound();
        StopAllCoroutines();
        SceneManager.LoadScene("MainMenu");
    }
}
