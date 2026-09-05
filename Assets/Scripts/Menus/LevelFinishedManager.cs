using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Manager to handle when the level is finished.
/// </summary>
public class LevelFinishedManager : MonoBehaviour
{
    private string previousLevelName;
    public Button PlayAgain;
    public TextMeshProUGUI bigText;
    public TextMeshProUGUI playerEXP;
    public TextMeshProUGUI defeatedEnemies;
    
    void Start()
    {
        if (GameManager.gm != null)
        {
            if (GameManager.gm.currentStageName != null || GameManager.gm.currentStageName != "")
            {
                previousLevelName = GameManager.gm.currentStageName;    
            }
            // This WILL have to change after making a proper ending scene. Can't just use the same one, it sucks.
            bigText.text = GameManager.gm.playerWon ? "STAGE CLEAR!" : "YOU DIED!";
            defeatedEnemies.text = $"You have defeated {GameManager.gm.enemiesDefeated} enemies.";
            playerEXP.text = $"Money earned: ${GameManager.gm.playerScore}";
        }        
        else // Debug/funny. The player should never ever be able to see this.
        {
            previousLevelName = "MainMenu";
            bigText.text = "You're not supposed to be here.";
            defeatedEnemies.text = $"You have defeated 0 enemies.";
            playerEXP.text = $"Money earned: $0 (poor)";
        }
        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(PlayAgain.gameObject);
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
