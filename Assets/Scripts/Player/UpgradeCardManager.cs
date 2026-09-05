using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UpgradeCardManager : MonoBehaviour
{
    [Header("References")]
    public GameObject cardPrefab;   // Prefab of the card (CardUI)
    public Transform cardParent;    // Panel where the cards will appear.

    [Header("SFX")]
    [SerializeField] private AudioSource upgradeChosen;

    private Action<UpgradeCard> onCardPicked;   // Callback that will reach the player.
    UpgradeCard firstCard = null;               // Saving the first card for controller purposes.

    // Muestra 'count' cartas y llama a onPicked cuando se elige una
    /// <summary>
    /// Shows cards randomly picked, obtained from the pram "upgrades".
    /// </summary>
    /// <param name="upgrades">List of upgrades chosen for the player during the level up.</param>
    /// <param name="onPicked"></param>
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
                card.Setup(upgrade, OnCardSelectedFromUI); 
                if (firstCard == null)
                {
                    firstCard = card;    
                }
            }
        }
        if (firstCard != null)
        {
            StartCoroutine(SelectFirstCardNextFrame(firstCard));
        }
    }

    private IEnumerator SelectFirstCardNextFrame(UpgradeCard firstCard)
    {
        yield return null; // Waits for 1 frame.
        EventSystem.current.SetSelectedGameObject(firstCard.gameObject);
    }


    /// <summary>
    /// Internal callback to invoke onCardPicked, reaching PlayerGameLogic.
    /// </summary>
    /// <param name="card">Upgrade Card selected by the player</param>
    private void OnCardSelectedFromUI(UpgradeCard card)
    {
        onCardPicked?.Invoke(card);

        upgradeChosen.Play();
    }

    /// <summary>
    /// Clears the cards.
    /// </summary>
    public void ClearCards()
    {
        if (cardParent == null) return;
        for (int i = cardParent.childCount - 1; i >= 0; i--)
        {
            Destroy(cardParent.GetChild(i).gameObject);
        }
    }
}
