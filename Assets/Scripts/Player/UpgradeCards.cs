using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UpgradeCard : MonoBehaviour, IPointerClickHandler
{
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI descriptionText;

    public UpgradeData upgradeData;

    public System.Action<UpgradeCard> onCardSelected;

    // Setup para strings (se mantiene)
    public void Setup(string title, string description, System.Action<UpgradeCard> callback)
    {
        titleText.text = title;
        descriptionText.text = description;
        onCardSelected = callback;
        upgradeData = null;
    }

    // Setup para UpgradeData REAL
    public void Setup(UpgradeData data, System.Action<UpgradeCard> callback)
    {
        upgradeData = data;
        titleText.text = data.title;
        descriptionText.text = data.description;
        onCardSelected = callback;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        onCardSelected?.Invoke(this);
    }
}
