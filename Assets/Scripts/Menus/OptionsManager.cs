using UnityEngine;
using UnityEngine.UI;

public class OptionsManager : MonoBehaviour
{
    public static OptionsManager optionsManager;

    public static Resolution[] validResolutions;

    private void Awake()
    {
        // Obtener solo resoluciones con refresco estándar (opcional)
        if (optionsManager == null)
        {
            optionsManager = this;
        }
        else
        {
            Destroy(gameObject);
        }
        validResolutions = Screen.resolutions;
        ApplySettingsFromPrefs();
    }

    public void ApplySettingsFromPrefs()
    {
        int width = PlayerPrefs.GetInt("res_w", Screen.currentResolution.width);
        int height = PlayerPrefs.GetInt("res_h", Screen.currentResolution.height);
        bool fullscreen = PlayerPrefs.GetInt("fullscreen", 1) == 1;

        Screen.SetResolution(width, height, fullscreen);
    }

    public void SetResolution(int width, int height)
    {
        PlayerPrefs.SetInt("res_w", width);
        PlayerPrefs.SetInt("res_h", height);
        PlayerPrefs.Save();
        ApplySettingsFromPrefs();
    }

    public void SetFullscreen(bool fullscreen)
    {
        PlayerPrefs.SetInt("fullscreen", fullscreen ? 1 : 0);
        PlayerPrefs.Save();
        ApplySettingsFromPrefs();
    }
}
