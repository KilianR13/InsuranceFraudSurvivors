using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuHandler : MonoBehaviour
{
    public AudioSource mainMenuMusic;
    public GameObject stageSelect;

    [Header("LevelSelect")]
    public Button playButton;
    public Outline playButtonOutline;
    public Button cancelButton;
    public Outline cancelButtonOutline;

    [Header("NormalMenu")]
    public Button levelSelectButton;
    public Outline LSButtonOutline;
    public Button optionsButton;
    public Outline optionsButtonOutline;
    public Button quitButton;
    public Outline quitButtonOutline;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        stageSelect.SetActive(false);
        mainMenuMusic.Play();
    }

    public void load()
    {
        SceneManager.LoadScene("Level1");
    }

    public void openPlayPanel()
    {
        stageSelect.SetActive(true);
    }

    public void closePlayPanel()
    {
        stageSelect.SetActive(false);
    }

    public void OnCancel()
    {
        if (stageSelect.activeSelf)
        {
            stageSelect.SetActive(false);    
        }
        
    }

    public void QuitGame()
    {
        Debug.Log("Saliendo del juego...");

        // Directiva de preprocesador
        #if UNITY_EDITOR
            // Si estamos en el editor de Unity, usamos el comando para detener el juego.
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            // Si estamos en un ejecutable (Build), cerramos la aplicación.
            Application.Quit();
        #endif
    }
}
