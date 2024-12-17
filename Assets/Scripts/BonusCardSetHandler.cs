using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class BonusCardSetHandler : MonoBehaviour
{
    [SerializeField]
    GameObject Discard;
    
    int slotCount = 0;
    int slotIndex = 0;
    List<GameObject> bonusCards = new List<GameObject>();
    List<GameObject> dispatchedBonusCards = new List<GameObject>();

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
            bonusCards.Add(bonusCard.gameObject);
            slotIndex++;
        }
        else
            Debug.LogWarning("Not enough slots for new bonus cards");
    }

    internal void ResizeCard(GameObject card)
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

    internal GameObject WithdrawCard()
    {
        if (bonusCards.Count > 0)
        {
            GameObject bonusCard = bonusCards[bonusCards.Count - 1];
            bonusCards.Remove(bonusCard);
            return bonusCard;
        }
        else return null;
    }

    internal void DiscardCard(GameObject bonusCard)
    {
        dispatchedBonusCards.Add(bonusCard);
        bonusCard.transform.SetParent(Discard.transform, false);
    }
}
