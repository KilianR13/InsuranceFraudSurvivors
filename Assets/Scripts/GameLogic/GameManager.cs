using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager gm;
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
        StartCoroutine(returnToMenu());
    }
    
    private IEnumerator returnToMenu()
    {
        yield return new WaitForSecondsRealtime(3);
        SceneManager.LoadScene("MainMenu");
        Time.timeScale = 1f;
    }
}
