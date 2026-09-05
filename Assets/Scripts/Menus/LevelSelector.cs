using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

/// <summary>
/// Handle the logic behind the level selector.
/// </summary>
public class LevelSelector : MonoBehaviour
{
    public LevelDatabase database;          // Should be able to add as many levels as I wish with the database.

    [Header("UI")]
    public Image previewImage;              // This will change during runtime with the images in the database.
    public TextMeshProUGUI levelNameText;   // So will this.

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
