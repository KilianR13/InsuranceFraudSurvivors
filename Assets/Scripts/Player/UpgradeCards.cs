using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UpgradeCard : MonoBehaviour, IPointerClickHandler
{
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI descriptionText;

    public System.Action<UpgradeCard> onCardSelected;

    public void Setup(string title, string description, System.Action<UpgradeCard> callback)
    {
        titleText.text = title;
        descriptionText.text = description;
        onCardSelected = callback;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        onCardSelected?.Invoke(this);
    }
}
