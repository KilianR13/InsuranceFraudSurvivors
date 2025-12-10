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

    [Header("Dropdown Joystick Navigation")]
    public float inputDelay = 0.2f; // Tiempo mínimo entre pasos de navegación con joystick

    private float inputTimer = 0f;

    private void Start()
    {
        SetupResolutionDropdown();
        LoadUIValues();
        resolutionDropdown.onValueChanged.RemoveAllListeners();
        resolutionDropdown.onValueChanged.AddListener(OnResolutionSelected);
    }

    void OnEnable()
    {
        // Selecciona automáticamente el botón de Guardar, para comodidad de los usuarios de mando.
        EventSystem.current.SetSelectedGameObject(firstButton.gameObject);
    }

    // ESTO NO FUNCIONA
    public void OnDropdownNav(InputValue value)
    {
        Debug.Log("Input Recibido");
        float inputY = value.Get<Vector2>().y;
        if (Mathf.Abs(inputY) > 0.2f)
        {
            inputTimer -= Time.unscaledDeltaTime;
            if (inputTimer <= 0f)
            {
                if (inputY > 0f)
                    MoveUp();
                else
                    MoveDown();

                inputTimer = inputDelay;
            }
        }
        else
        {
            inputTimer = 0f;
        }
    }

    private void MoveUp()
    {
        int index = Mathf.Max(0, resolutionDropdown.value - 1);
        resolutionDropdown.value = index;
        ScrollToSelected(index);

        resolutionDropdown.RefreshShownValue(); // FORZAR actualización visual
    }

    private void MoveDown()
    {
        int index = Mathf.Min(resolutionDropdown.options.Count - 1, resolutionDropdown.value + 1);
        resolutionDropdown.value = index;
        ScrollToSelected(index);

        resolutionDropdown.RefreshShownValue(); // FORZAR actualización visual
        
    }


    private void SetupResolutionDropdown()
    {
        resolutionDropdown.ClearOptions();

        var resolutions = OptionsManager.validResolutions;
        var options = new List<string>();

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
        fullscreenToggle.onValueChanged.RemoveAllListeners();
        fullscreenToggle.isOn = PlayerPrefs.GetInt("fullscreen", 1) == 1;
        fullscreenToggle.onValueChanged.AddListener(OnFullscreenToggle);
    }

    public void OnResolutionSelected(int index)
    {
        Resolution selected = OptionsManager.validResolutions[index];
        Debug.Log($"Seleccionado: {selected.width}, {selected.height}");
        optionsManager.SetResolution(selected.width, selected.height);
        resolutionDropdown.value = index;
        // if (resolutionDropdown.template.gameObject.activeSelf)
        //     ScrollToSelected(index);
    }

    public void OnFullscreenToggle(bool value)
    {
        optionsManager.SetFullscreen(value); // Se llama desde aquí
    }

    private void ScrollToSelected(int index)
    {
        ScrollRect scrollRect = resolutionDropdown.template.GetComponentInChildren<ScrollRect>();
        if (scrollRect == null) return;

        int total = resolutionDropdown.options.Count;
        if (total <= 1) return;

        float normalizedPos = 1f - (float)index / (total - 1);
        scrollRect.verticalNormalizedPosition = normalizedPos;
    }

}
