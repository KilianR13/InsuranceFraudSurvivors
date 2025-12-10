using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    public OptionsManager optionsManager;
    public TMP_Dropdown resolutionDropdown;  // Arrastras el Dropdown aquí
    public Toggle fullscreenToggle;      // Arrastras el Toggle aquí

    private void Start()
    {
        SetupResolutionDropdown();
        LoadUIValues();
    }

    private void SetupResolutionDropdown()
    {
        resolutionDropdown.ClearOptions();

        var resolutions = OptionsManager.validResolutions;
        var options = new System.Collections.Generic.List<string>();

        int currentIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string resString = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(resString);

            // Detectar resolución actual para mostrarla seleccionada
            if (resolutions[i].width == Screen.width && resolutions[i].height == Screen.height)
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
        fullscreenToggle.isOn = PlayerPrefs.GetInt("fullscreen", 1) == 1;
    }

    public void OnResolutionSelected(int index)
    {
        Resolution selected = OptionsManager.validResolutions[index];
        optionsManager.SetResolution(selected.width, selected.height);
    }

    public void OnFullscreenToggle(bool value)
    {
        optionsManager.SetFullscreen(value);
    }
}
