using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BonusCardHandler : MonoBehaviour
{
    [SerializeField]
    GameObject[] slots;
    [SerializeField]
    GameObject tempSlot;

    public List<GameObject> bonusCards = new();
/*    [SerializeField]
    BonusCardSetHandler bonusCardSetHandler;*/

    // Add card to an empty slot, and add to list
    // TODO: handle error if no slot is empty
    internal void AddCard(GameObject card)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].transform.childCount == 0)
            {
                card.transform.SetParent(slots[i].transform);
                ResizeCard(card);
                bonusCards.Add(card);
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
            if (slot.TryGetComponentInChildren<BonusCard>(out BonusCard bonusCard))
                bonusCard.SetSelectable();
        }
    }

    internal void ShowTempSlot(bool isActive)
    {
        tempSlot.SetActive(isActive);
    }

    internal void ReorderCards()
    {
        List<GameObject> temp = new();
        temp.AddRange(bonusCards);
        bonusCards.Clear();

        foreach(GameObject card in temp)
        {
            card.transform.SetParent(null);
        }
        
        foreach(GameObject card in temp)
        {
            AddCard(card);
        }
    }
}
