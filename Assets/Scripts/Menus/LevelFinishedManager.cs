using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelFinishedManager : MonoBehaviour
{
    private string previousLevelName;
    public TextMeshProUGUI bigText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Para prevenir errores
        if (GameManager.gm.currentStageName != null || GameManager.gm.currentStageName != "")
        {
            previousLevelName = GameManager.gm.currentStageName;    
        }
        else
        {
            previousLevelName = "MainMenu";
        }
        
        bigText.text = GameManager.gm.playerWon ? "STAGE COMPLETE!" : "YOU DIED!";
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
