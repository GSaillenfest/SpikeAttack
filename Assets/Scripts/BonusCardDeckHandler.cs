using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusCardDeckHandler : MonoBehaviour
{
    int slotCount = 0;
    int slotIndex = 0;
    List<BonusCard> bonusCards = new List<BonusCard>();

    private void Start()
    {
        slotCount = transform.GetChild(0).childCount;
    }

    internal void OrganizeCard(GameObject bonusCard)
    {
        if (slotIndex < slotCount)
        {
            bonusCard.transform.SetParent(transform.GetChild(0).GetChild(slotIndex));
            ResizeCard(bonusCard);
            bonusCards.Add(bonusCard.GetComponentInChildren<BonusCard>());
            slotIndex++;
        }
        else
            Debug.LogWarning("Not enough slots for new bonus cards");
    }

    void ResizeCard(GameObject card)
    {
        RectTransform rectTransform = card.GetComponent<RectTransform>();
        if (rectTransform != null)
        {
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
            rectTransform.offsetMin = Vector2.zero;
            rectTransform.offsetMax = Vector2.zero;
            rectTransform.localScale = Vector3.one;
        }
    }
}
