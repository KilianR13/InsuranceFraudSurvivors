using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Public class meant for buttons
/// </summary>
public class ArrowHighlightToggle : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
{
    public GameObject arrowOutline;
    void Start()
    {
        if (arrowOutline != null)
            arrowOutline.SetActive(false);
    }

    // Para el ratón
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (arrowOutline != null)
            arrowOutline.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (arrowOutline != null)
            arrowOutline.SetActive(false);
    }

    // Para teclado/gamepad
    public void OnSelect(BaseEventData eventData)
    {
        if (arrowOutline != null)
            arrowOutline.SetActive(true);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        if (arrowOutline != null)
            arrowOutline.SetActive(false);
    }

}
