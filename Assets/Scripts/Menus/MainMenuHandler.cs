using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuHandler : MonoBehaviour
{
    public AudioSource mainMenuMusic;
    public GameObject stageSelect;
    public GameObject optionsMenu;

    [Header("LevelSelect")]
    public Button playButton;
    public Button cancelButton;

    [Header("NormalMenu")]
    public Button levelSelectButton;
    public Button optionsButton;
    public Button quitButton;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        EventSystem.current.SetSelectedGameObject(levelSelectButton.gameObject);
        stageSelect.SetActive(false);
        optionsMenu.SetActive(false);
        mainMenuMusic.Play();
    }

    public void openPlayPanel()
    {
        EventSystem.current.SetSelectedGameObject(playButton.gameObject);
        stageSelect.SetActive(true);
    }

    public void closePlayPanel()
    {
        EventSystem.current.SetSelectedGameObject(levelSelectButton.gameObject);
        stageSelect.SetActive(false);
    }

    public void openOptions()
    {
        optionsMenu.SetActive(true);
    }

    public void closeOptions()
    {
        EventSystem.current.SetSelectedGameObject(levelSelectButton.gameObject);
        optionsMenu.SetActive(false);
    }

    public void OnCancel()
    {
        if (stageSelect.activeSelf)
        {
            closePlayPanel();   
        }
        if (optionsMenu.activeSelf)
        {
            closeOptions();
        }
        
    }

    public void QuitGame()
    {
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
