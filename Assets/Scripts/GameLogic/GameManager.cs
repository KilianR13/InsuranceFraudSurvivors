using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager gm;
    [SerializeField] private AudioSource playerDefeatedSFX;
    public AudioSource BackgroundMusicSFX;
    public string currentStageName;
    public bool playerWon = false;
    public int playerLevel;
    public int playerScore;
    public int enemiesDefeated = 0;
    public bool gamePaused;

    void Awake()
    {
        if (gm == null)
        {
            gm = this;    
        }
        else
        {
            Destroy(gameObject);
        }
        
        DontDestroyOnLoad(gm);
    }

    public void StartGame()
    {
        gamePaused = false;
        currentStageName = SceneManager.GetActiveScene().name;
        playerLevel = 0;
        playerScore = 0;
        enemiesDefeated = 0;
        playerWon = true;
        BackgroundMusicSFX.volume = 0.4f;
        BackgroundMusicSFX.Play();
    }

    public void dynamicMusic()
    {
        if (gamePaused)
        {
            BackgroundMusicSFX.volume = 0.1f;
        }
        else
        {
            BackgroundMusicSFX.volume = 0.4f;
        }
    }

    public void StageCompleted(bool playerWon)
    {
        BackgroundMusicSFX.Stop();
        if (!playerWon)
        {
            playerDefeatedSFX.Play();
        }
        this.playerWon = playerWon;
        Time.timeScale = 0f;
        StartCoroutine(returnToMenu());
    }

    private IEnumerator returnToMenu()
    {
        yield return new WaitForSecondsRealtime(3);
        SceneManager.LoadScene("LevelFinished");
        Time.timeScale = 1f;
    }
}
