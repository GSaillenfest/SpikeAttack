using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamClass : MonoBehaviour
{

    public List<GameObject> playerList;
    public PlayerDeckOnField deckOnField;

    public TeamClass()
    {
        playerList = new List<GameObject>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void AddPlayerToTeam(GameObject playerCard)
    {
        playerList.Add(playerCard);
    }

    internal void SetBlockPhase()
    {
        deckOnField.SetBlockPhase();
    }

    internal void SetSelectableCardAction(int actionIndex, int[] selectedCardSlots)
    {
        deckOnField.SetSelectableCardAction(actionIndex, selectedCardSlots);
    }

    internal void ValidateActionCombo(int slotIndex, int actionIndex)
    {
        deckOnField.ValidateActionCombo(slotIndex, actionIndex);
    }

    internal void ValidateBlock(int slotIndex)
    {
        deckOnField.ValidateBlock(slotIndex);
    }    
    
    internal void ValidateServe()
    {
        deckOnField.ValidateServe();
    }

    internal void SetAllSelectableCardField(bool isSelectable)
    {
        deckOnField.SetAllSelectableCardAction(isSelectable);
    }

    internal void SetServePhase()
    {
        deckOnField.SetServePhase();
    }

    internal int GetServeValue()
    {
        return deckOnField.GetServeValue();
    }

    internal VolleyPlayer GetPlayerOnField(int slotIndex)
    {
        return deckOnField.GetPlayer(slotIndex);
    }
}
