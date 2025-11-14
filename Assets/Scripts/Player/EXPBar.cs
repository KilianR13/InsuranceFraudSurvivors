using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EXPBar : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private TextMeshProUGUI currentLVL;


    public void UpdateEXPBar(float currentValue, float maxValue)
    {
        slider.value = currentValue / maxValue;
    }

    public void UpdateLevel(int newLevel)
    {
        currentLVL.text = $"Lv {newLevel}";
    }

}
