using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeckOnSidelines : MonoBehaviour
{
    [SerializeField]
    public GameObject[] deckSlots;

    public List<VolleyPlayer> playersOnSidelines = new();

    private void OnEnable()
    {
        RefreshDeck();
    }

    public void RefreshDeck()
    {
        playersOnSidelines.Clear();
        VolleyPlayer[] childList = gameObject.GetComponentsInChildren<VolleyPlayer>();
        foreach (VolleyPlayer child in childList)
        {
            playersOnSidelines.Add(child);
        }
        SetSlotIndex();
    }

    public void SetSlotIndex()
    {
        for (int i = 0; i < playersOnSidelines.Count; i++)
        {
            playersOnSidelines[i].slotIndex = i + 6;
        }
    }

    internal void SetAllSelectableCard(bool isSelectable)
    {
        foreach (VolleyPlayer player in playersOnSidelines)
        {
            player.SetSelectable(isSelectable);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        RefreshDeck();
    }

    internal VolleyPlayer GetPlayer(int slotIndex)
    {
        return playersOnSidelines[slotIndex - 6];
    }

}
