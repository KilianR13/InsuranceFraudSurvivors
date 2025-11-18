using System;
using UnityEngine;

public class UpgradeCardManager : MonoBehaviour
{
    [Header("References")]
    public GameObject cardPrefab;   // prefab de la carta (CardUI)
    public Transform cardParent;    // panel donde se instancian (layout horizontal)

    private Action<UpgradeCard> onCardPicked; // callback que avisa al jugador

    // Muestra 'count' cartas y llama a onPicked cuando se elige una
    public void ShowCards(int count, Action<UpgradeCard> onPicked)
    {
        ClearCards();

        onCardPicked = onPicked;

        for (int i = 0; i < count; i++)
        {
            GameObject cardGO = Instantiate(cardPrefab, cardParent);
            UpgradeCard card = cardGO.GetComponent<UpgradeCard>();
            if (card != null)
            {
                // Rellena con contenido de prueba (más tarde usa datos reales)
                string title = $"Mejora {i + 1}";
                string desc = "Descripción de ejemplo";
                card.Setup(title, desc, OnCardSelectedFromUI);
            }
        }
    }

    // Callback interno cuando una carta es seleccionada por click
    private void OnCardSelectedFromUI(UpgradeCard card)
    {
        // Llamamos al callback del cliente (PlayerGameLogic)
        onCardPicked?.Invoke(card);

        // Limpiamos y ocultamos las cartas del panel
        ClearCards();
    }

    // Borra las cartas instanciadas (útil para cerrar)
    public void ClearCards()
    {
        if (cardParent == null) return;
        for (int i = cardParent.childCount - 1; i >= 0; i--)
        {
            Destroy(cardParent.GetChild(i).gameObject);
        }
    }
}
