using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager gm;
    [SerializeField] private AudioSource playerDefeatedSFX;
    [SerializeField] private AudioSource StageCompletedSFX;
    public string currentStageName;
    public bool playerWon = false;
    public int playerLevel;
    public int playerScore;
    public int enemiesDefeated = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gm = this;
        currentStageName = SceneManager.GetActiveScene().name;
        Debug.Log(currentStageName);
        playerLevel = 0;
        playerScore = 0;
        enemiesDefeated = 0;
    }

    public void StageCompleted(bool playerWon)
    {
        if (!playerWon)
        {
            playerDefeatedSFX.Play();
        }
        this.playerWon = playerWon;
        Time.timeScale = 0f;
        StartCoroutine(returnToMenu());
    }
    
    // private IEnumerator StageCompletedTimer()
    // {
    //     StageCompletedSFX.Play();
    //     yield return new WaitForSecondsRealtime(StageCompletedSFX.clip.length);
    //     SceneManager.LoadScene("LevelFinished");
    //     Time.timeScale = 1f;
    // }

    private IEnumerator returnToMenu()
    {
        yield return new WaitForSecondsRealtime(3);
        SceneManager.LoadScene("LevelFinished");
        Time.timeScale = 1f;
    }
}
