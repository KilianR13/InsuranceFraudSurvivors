using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UpgradeCardManager : MonoBehaviour
{
    [Header("References")]
    public GameObject cardPrefab;   // prefab de la carta (CardUI)
    public Transform cardParent;    // panel donde se instancian (layout horizontal)

    [Header("SFX")]
    [SerializeField] private AudioSource upgradeChosen;

    private Action<UpgradeCard> onCardPicked; // callback que avisa al jugador
    UpgradeCard firstCard = null; // Para guardar la primera carta

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
                if (firstCard == null)
                {
                    firstCard = card;    
                }
            }
        }
        if (firstCard != null)
        {
            // Asegúrate de que tu UpgradeCard tenga un Button o Selectable
            StartCoroutine(SelectFirstCardNextFrame(firstCard));
        }
    }

    private IEnumerator SelectFirstCardNextFrame(UpgradeCard firstCard)
    {
        yield return null; // espera un frame
        EventSystem.current.SetSelectedGameObject(firstCard.gameObject);
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
