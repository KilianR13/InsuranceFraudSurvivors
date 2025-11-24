using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UpgradeCard : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI descriptionText;

    public UpgradeData upgradeData;

    public AudioSource hoverSFX;

    public Outline coolOutline;

    public System.Action<UpgradeCard> onCardSelected;

    void Start()
    {
        coolOutline.enabled = false;
    }

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

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (hoverSFX != null)
        {
            hoverSFX.Play();
        }
        coolOutline.enabled = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        coolOutline.enabled = false;
    }
}
