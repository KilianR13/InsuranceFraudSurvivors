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
        if (GameManager.gm != null)
        {
            if (GameManager.gm.currentStageName != null || GameManager.gm.currentStageName != "")
            {
                previousLevelName = GameManager.gm.currentStageName;    
            }    
            bigText.text = GameManager.gm.playerWon ? "STAGE COMPLETE!" : "YOU DIED!";
        }        
        else
        {
            previousLevelName = "MainMenu";
            bigText.text = "You shouldn't be here.";
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
