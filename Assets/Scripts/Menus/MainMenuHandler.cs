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
}
