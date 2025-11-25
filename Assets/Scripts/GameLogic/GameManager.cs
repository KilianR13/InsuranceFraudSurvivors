using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager gm;
    [SerializeField] private AudioSource playerDefeatedSFX;
    [SerializeField] private AudioSource StageCompletedSFX;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (gm == null)
        {
            gm = this;
        }
        else
        {
            Destroy(gameObject); // evita duplicados si hay otro GameManager
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void loseGame()
    {
        Time.timeScale = 0f;
        playerDefeatedSFX.Play();
        StartCoroutine(returnToMenu());
    }

    public void StageCompleted()
    {
        // Time.timeScale = 0f;
        // StartCoroutine(StageCompletedTimer());
    }
    
    private IEnumerator StageCompletedTimer()
    {
        StageCompletedSFX.Play();
        yield return new WaitForSecondsRealtime(StageCompletedSFX.clip.length);
        SceneManager.LoadScene("MainMenu");
        Time.timeScale = 1f;
    }

    private IEnumerator returnToMenu()
    {
        yield return new WaitForSecondsRealtime(3);
        SceneManager.LoadScene("MainMenu");
        Time.timeScale = 1f;
    }
}
