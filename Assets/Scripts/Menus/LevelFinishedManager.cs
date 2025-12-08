using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelFinishedManager : MonoBehaviour
{
    private string previousLevelName;
    private int enemiesDefeatedPreviousLevel;
    public TextMeshProUGUI bigText;
    public TextMeshProUGUI playerEXP;
    public TextMeshProUGUI defeatedEnemies;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Para prevenir errores
        if (GameManager.gm != null)
        {
            if (GameManager.gm.currentStageName != null || GameManager.gm.currentStageName != "")
            {
                previousLevelName = GameManager.gm.currentStageName;    
            }    
            bigText.text = GameManager.gm.playerWon ? "STAGE COMPLETE!" : "YOU DIED!";
            defeatedEnemies.text = $"You have defeated {GameManager.gm.enemiesDefeated} enemies.";
            playerEXP.text = $"Money earned: ${GameManager.gm.playerScore}";
        }        
        else
        {
            previousLevelName = "MainMenu";
            bigText.text = "You shouldn't be here.";
            defeatedEnemies.text = $"You have defeatd 0 enemies.";
            playerEXP.text = $"Money earned: $0 (poor)";
        }
    }
    
    public void restartPreviousLevel()
    {
        SceneManager.LoadScene(previousLevelName);
    }
    public void returnToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
