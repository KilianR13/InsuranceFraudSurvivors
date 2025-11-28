using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuHandler : MonoBehaviour
{
    public AudioSource mainMenuMusic;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mainMenuMusic.Play();
    }

    public void load()
    {
        SceneManager.LoadScene("CarScene");
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
