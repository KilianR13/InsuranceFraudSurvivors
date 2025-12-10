using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    public OptionsManager optionsManager;
    public TMP_Dropdown resolutionDropdown;    
    public Toggle fullscreenToggle;
    public Button firstButton;

    private int initialWidth;
    private int initialHeight;
    private bool initialFullscreen;
    private int initialDropdownIndex;

    private void Start()
    {
        SetupResolutionDropdown();
        LoadUIValues();

        resolutionDropdown.onValueChanged.RemoveAllListeners();
        resolutionDropdown.onValueChanged.AddListener(OnResolutionSelected);
    }

    void OnEnable()
    {
        // Guardar estado REAL de la pantalla antes de abrir el menú
        initialWidth = Screen.width;
        initialHeight = Screen.height;
        initialFullscreen = Screen.fullScreen;

        initialDropdownIndex = resolutionDropdown.value;

        // Seleccionar botón para mando
        EventSystem.current.SetSelectedGameObject(firstButton.gameObject);
    }

    private void SetupResolutionDropdown()
    {
        resolutionDropdown.ClearOptions();

        var resolutions = OptionsManager.validResolutions;
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

    private void LoadUIValues()
    {
        fullscreenToggle.onValueChanged.RemoveAllListeners();
        fullscreenToggle.isOn = PlayerPrefs.GetInt("fullscreen", 1) == 1;
    }

    public void OnResolutionSelected(int index)
    {
        // Solo actualizar UI. NO aplicar resolución aquí.
        Debug.Log($"Resolución seleccionada en UI: {resolutionDropdown.options[index].text}");
    }

    public void OnFullscreenToggle(bool value)
    {
        // Solo UI. NO aplicar resoluciones aquí.
    }

    public void SaveChanges()
    {
        Resolution selected = OptionsManager.validResolutions[resolutionDropdown.value];
        bool fullscreen = fullscreenToggle.isOn;

        // Aplicar al juego
        optionsManager.ApplyResolution(selected.width, selected.height, fullscreen);

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
        optionsManager.ApplyResolution(initialWidth, initialHeight, initialFullscreen);

        // Restaurar UI
        resolutionDropdown.value = initialDropdownIndex;
        resolutionDropdown.RefreshShownValue();

        fullscreenToggle.isOn = initialFullscreen;

        Debug.Log("Cambios revertidos sin guardar.");
    }
}
