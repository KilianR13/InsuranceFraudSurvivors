using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EXPBar : MonoBehaviour
{   
    [Header("UI")]
    [SerializeField] private Slider slider;
    [SerializeField] private TextMeshProUGUI currentLVL;
    [SerializeField] private Image fillImage;

    

    private Coroutine rainbowCoroutine;

    private Color normalColor = Color.blue; //00F6FF


    public void UpdateEXPBar(float currentValue, float maxValue)
    {
        slider.value = currentValue / maxValue;
    }

    public void UpdateLevel(int newLevel)
    {
        currentLVL.text = $"Lv {newLevel}";
    }

    public void StartRainbow()
    {
        if (rainbowCoroutine != null)
            StopCoroutine(rainbowCoroutine);

        rainbowCoroutine = StartCoroutine(RainbowEffect());
    }

    public void StopRainbow()
    {
        if (rainbowCoroutine != null)
        {
            StopCoroutine(rainbowCoroutine);
            rainbowCoroutine = null;
        }

        if (fillImage != null)
            fillImage.color = normalColor;
    }

    private IEnumerator RainbowEffect()
    {
        float hue = 0f;

        while (true)
        {
            hue += Time.unscaledDeltaTime * 0.5f; // Usamos unscaledDeltaTime
            if (hue > 1f) hue = 0f;

            fillImage.color = Color.HSVToRGB(hue, 1f, 1f);

            yield return null;
        }
    }



}
