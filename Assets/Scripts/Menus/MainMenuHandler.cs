using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuHandler : MonoBehaviour
{
    public AudioSource mainMenuMusic;
    public GameObject stageSelect;

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
        mainMenuMusic.Play();
    }

    public void load()
    {
        SceneManager.LoadScene("Level1");
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

    public void OnCancel()
    {
        if (stageSelect.activeSelf)
        {
            EventSystem.current.SetSelectedGameObject(levelSelectButton.gameObject);
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
