using System;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeCardManager : MonoBehaviour
{
    [Header("References")]
    public GameObject cardPrefab;   // prefab de la carta (CardUI)
    public Transform cardParent;    // panel donde se instancian (layout horizontal)

    [Header("SFX")]
    [SerializeField] private AudioSource upgradeChosen;

    private Action<UpgradeCard> onCardPicked; // callback que avisa al jugador

    // Muestra 'count' cartas y llama a onPicked cuando se elige una
    public void ShowCards(List<UpgradeData> upgrades, Action<UpgradeCard> onPicked)
    {
        ClearCards();
        onCardPicked = onPicked;

        foreach (var upgrade in upgrades)
        {
            GameObject cardGO = Instantiate(cardPrefab, cardParent);
            UpgradeCard card = cardGO.GetComponent<UpgradeCard>();

            if (card != null)
            {
                card.Setup(upgrade, OnCardSelectedFromUI);  // << REAL DATA
            }
        }
    }

    // Callback interno cuando una carta es seleccionada por click
    private void OnCardSelectedFromUI(UpgradeCard card)
    {
        // Llamamos al callback del cliente (PlayerGameLogic)
        onCardPicked?.Invoke(card);

        upgradeChosen.Play();
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
