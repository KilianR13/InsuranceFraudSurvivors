using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Class that handles all the logic of the options menu.
/// </summary>
public class OptionsMenu : MonoBehaviour
{
    public TMP_Dropdown resolutionDropdown;    
    public Toggle fullscreenToggle;
    public Button firstButton;
    public static Resolution[] validResolutions;

    private int initialWidth;
    private int initialHeight;
    private bool initialFullscreen;
    private int initialDropdownIndex;

    void Awake()
    {
        validResolutions = GetUniqueResolutions();
        ApplySettingsFromPrefs(); 
    }

    private void Start()
    {
        SetupResolutionDropdown();
        LoadUIValues();

        resolutionDropdown.onValueChanged.RemoveAllListeners();
        resolutionDropdown.onValueChanged.AddListener(OnResolutionSelected);
    }

    void OnEnable()
    {
        // Saves the current width, height, and fullscreen status in case the player regrets changing anything, so it can be reversed.
        initialWidth = Screen.width;
        initialHeight = Screen.height;
        initialFullscreen = Screen.fullScreen;

        initialDropdownIndex = resolutionDropdown.value;

        // Selector so it's compatible with a controller.
        EventSystem.current.SetSelectedGameObject(firstButton.gameObject);
    }

    private void SetupResolutionDropdown()
    {
        resolutionDropdown.ClearOptions();

        var resolutions = validResolutions;
        List<string> options = new List<string>();

        int currentIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string text = $"{resolutions[i].width} x {resolutions[i].height}";
            options.Add(text);

            if (resolutions[i].width == Screen.width &&
                resolutions[i].height == Screen.height)
            {
                currentIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentIndex;
        resolutionDropdown.RefreshShownValue();
    }

    /// <summary>
    /// Creates a list of all available resolutions supported by Unity and returns them as an array.
    /// </summary>
    /// <returns>An array of the base resolutions supported by Unity.</returns>
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

    /// <summary>
    /// Function that loads the resolution settings inside PlayerPrefs.
    /// </summary>
    public void ApplySettingsFromPrefs()
    {
        int width = PlayerPrefs.GetInt("res_w", Screen.currentResolution.width);
        int height = PlayerPrefs.GetInt("res_h", Screen.currentResolution.height);
        bool fullscreen = PlayerPrefs.GetInt("fullscreen", 1) == 1;

        Screen.fullScreenMode = fullscreen ?
            FullScreenMode.ExclusiveFullScreen :
            FullScreenMode.Windowed;

        Screen.SetResolution(width, height, fullscreen);
    }

    /// <summary>
    /// Applies the changes selected by the user.
    /// </summary>
    /// <param name="width">Width of the program's screen</param>
    /// <param name="height">Height of the program's screen</param>
    /// <param name="fullscreen">Boolean for setting the program's fullscreen to "fullscreen" or "windowed"</param>
    public void ApplyResolution(int width, int height, bool fullscreen)
    {
        Screen.fullScreenMode = fullscreen ?
            FullScreenMode.ExclusiveFullScreen :
            FullScreenMode.Windowed;

        Screen.SetResolution(width, height, fullscreen);
    }

    private void LoadUIValues()
    {
        fullscreenToggle.onValueChanged.RemoveAllListeners();
        fullscreenToggle.isOn = PlayerPrefs.GetInt("fullscreen", 1) == 1;
    }

    /// <summary>
    /// Function only used as debug for the dropdown menu. Useless otherwise.
    /// </summary>
    /// <param name="index">Index of the dropdown selected</param>
    public void OnResolutionSelected(int index)
    {
        // DEBUG ONLY
        Debug.Log($"Resolución seleccionada en UI: {resolutionDropdown.options[index].text}");
    }

    /// <summary>
    /// Empty function. Doesn't do anything.
    /// </summary>
    /// <param name="value">Toggle between full screen (true) or windowed (false)</param>
    public void OnFullscreenToggle(bool value)
    {
        // Empty function.
    }

    public void SaveChanges()
    {
        Resolution selected = validResolutions[resolutionDropdown.value];
        bool fullscreen = fullscreenToggle.isOn;

        // Aplicar al juego
        ApplyResolution(selected.width, selected.height, fullscreen);

        // Guardar prefs
        PlayerPrefs.SetInt("res_w", selected.width);
        PlayerPrefs.SetInt("res_h", selected.height);
        PlayerPrefs.SetInt("fullscreen", fullscreen ? 1 : 0);
        PlayerPrefs.Save();

        Debug.Log("Cambios guardados correctamente.");
    }

    public void RevertChanges()
    {
        // Restaurar resolución real original
        ApplyResolution(initialWidth, initialHeight, initialFullscreen);

        // Restaurar UI
        resolutionDropdown.value = initialDropdownIndex;
        resolutionDropdown.RefreshShownValue();

        fullscreenToggle.isOn = initialFullscreen;

        Debug.Log("Cambios revertidos sin guardar.");
    }
}
