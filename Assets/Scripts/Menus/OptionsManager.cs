using System.Collections.Generic;
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
        validResolutions = GetUniqueResolutions();
        ApplySettingsFromPrefs();
    }

    private Resolution[] GetUniqueResolutions()
    {
        Resolution[] allRes = Screen.resolutions;
        List<Resolution> filtered = new List<Resolution>();
        HashSet<string> seen = new HashSet<string>();

        foreach (var res in allRes)
        {
            string key = res.width + "x" + res.height;

            // Evita resoluciones duplicadas (solo una por tamaño)
            if (!seen.Contains(key))
            {
                seen.Add(key);
                filtered.Add(res);
            }
        }

        return filtered.ToArray();
    }

    public void ApplySettingsFromPrefs()
    {
        int width = PlayerPrefs.GetInt("res_w", Screen.currentResolution.width);
        int height = PlayerPrefs.GetInt("res_h", Screen.currentResolution.height);
        bool fullscreen = PlayerPrefs.GetInt("fullscreen", 1) == 1;

        Screen.fullScreenMode = fullscreen 
            ? FullScreenMode.ExclusiveFullScreen 
            : FullScreenMode.Windowed;

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
