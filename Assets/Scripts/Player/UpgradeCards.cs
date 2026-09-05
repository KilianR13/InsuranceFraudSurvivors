using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Upgrade the player can pick.
/// </summary>
public class UpgradeCard : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler, ISubmitHandler
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

    /// <summary>
    /// Assigns the necessary data to be displayed on this Card. The information for the card is obtained from the Data.
    /// </summary>
    /// <param name="data">Data about the upgrade</param>
    /// <param name="callback">Action of selecting the Card.</param>
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

    public void OnSubmit(BaseEventData eventData)
    {
        onCardSelected?.Invoke(this);
    }

    public void OnSelect(BaseEventData eventData)
    {
        if (hoverSFX != null)
        {
            hoverSFX.Play();
        }
        coolOutline.enabled = true;
    }

    public void OnDeselect(BaseEventData eventData)
    {
        coolOutline.enabled = false;
    }
}
