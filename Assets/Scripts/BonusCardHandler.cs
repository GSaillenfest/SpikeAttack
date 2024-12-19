using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusCardHandler : MonoBehaviour
{
    [SerializeField]
    GameObject[] slots;
/*    [SerializeField]
    BonusCardSetHandler bonusCardSetHandler;*/

    internal void AddCard(GameObject card)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].transform.childCount == 0)
            {
                card.transform.SetParent(slots[i].transform);
                ResizeCard(card);
                break;
            }
        }
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
            rectTransform.anchoredPosition3D = Vector3.zero;
        }
    }

    internal void CheckCardSelectable()
    {
        foreach (GameObject slot in slots)
        {
            if (slot.GetComponentInChildren<BonusCard>())
                slot.GetComponentInChildren<BonusCard>().SetSelectable();
        }
    }
}
