using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelSelector : MonoBehaviour
{
    public LevelDatabase database;

    [Header("UI")]
    public Image previewImage;
    public TextMeshProUGUI levelNameText;

    private int index = 0;

    void Start()
    {
        UpdateUI();
    }

    public void NextLevel()
    {
        index++;
        if (index >= database.levels.Length)
        {
            index = 0;
        }
        UpdateUI();
    }

    public void PrevLevel()
    {
        index--;
        if (index < 0)
        {
            index = database.levels.Length - 1;
        }
        UpdateUI();
    }

    void UpdateUI()
    {
        var data = database.levels[index];
        previewImage.sprite = data.previewImage;
        levelNameText.text = data.levelDisplayName;
    }

    public void PlaySelectedLevel()
    {
        SceneManager.LoadScene(database.levels[index].sceneName);
    }
}
