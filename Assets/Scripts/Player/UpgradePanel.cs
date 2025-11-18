using UnityEngine;

public class UpgradePanel : MonoBehaviour
{
    public GameObject cardPrefab;
    public Transform cardParent;

    public void ShowOneCard()
    {
        GameObject card = Instantiate(cardPrefab, cardParent);

        var ui = card.GetComponent<UpgradeCard>();
    }
}
