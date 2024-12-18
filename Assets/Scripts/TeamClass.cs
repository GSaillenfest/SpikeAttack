using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TeamClass : MonoBehaviour
{
    [SerializeField]
    BonusCardSetHandler bonusCardSetHandler;
    [SerializeField]
    BonusCardHandler bonusCardHandler;

    public List<GameObject> playerList;
    public PlayerDeckOnField deckOnField;
    public PlayerDeckOnSidelines deckOnSide;

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

    internal void AddBonusCard(int numberOfCards)
    {
        for (int i = 0; i < numberOfCards; i++)
        {
            GameObject card = bonusCardSetHandler.WithdrawCard();
            if (card != null)
            {
                bonusCardHandler.AddCard(card);
            }
        }
    }

    internal void SetBlockSelectionPhase()
    {
        deckOnField.SetBlockSelectionPhase();
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

    internal void SetServePhase(bool blur = false)
    {
        deckOnField.SetServePhase();
    }

    internal int GetServeValue()
    {
        return deckOnField.GetServeValue();
    }

    internal VolleyPlayer GetPlayerBySlotIndex(int slotIndex)
    {
        if (slotIndex < 0) return null;
        if (slotIndex < 6)
            return deckOnField.GetPlayer(slotIndex);
        else
            return deckOnSide.GetPlayer(slotIndex);
    }

    internal void ResetStatus()
    {
        deckOnField.ResetStatus();
    }

    internal void RotateFieldCards()
    {
        deckOnField.RotatePlayerCards();
    }

    internal void SetAllSelectableCardOnSide(bool isSelectable)
    {
        deckOnSide.SetAllSelectableCard(isSelectable);
    }

    // Switch playerCards and set them unselectable
    internal void SwitchTwoCardPlayerSlot(int slotA, int slotB)
    {
        GameObject fieldPlayer;
        GameObject sidePlayer;
        int fieldSlot = -1;
        int sideSlot = -1;

        if (slotA < 6)
        {
            fieldSlot = slotA;
            sideSlot = slotB;
        }
        else
        {
            fieldSlot = slotB;
            sideSlot = slotA;
        }

        deckOnField.GetPlayer(fieldSlot).ResetActions();

        deckOnField.GetPlayer(fieldSlot).SetSelectable(false);
        deckOnSide.GetPlayer(sideSlot).SetSelectable(false);

        fieldPlayer = deckOnField.GetPlayer(fieldSlot).gameObject;
        sidePlayer = deckOnSide.GetPlayer(sideSlot).gameObject;

        fieldPlayer.transform.SetParent(deckOnSide.deckSlots[sideSlot - 6].transform, false);
        sidePlayer.transform.SetParent(deckOnField.deckSlots[fieldSlot].transform, false);

        deckOnField.RefreshDeck();
        deckOnSide.RefreshDeck();
    }

    internal void ShowSidelines(bool isShow)
    {
        deckOnSide.gameObject.SetActive(isShow);
    }

    internal void OnStart()
    {
        deckOnField.OnStart();
    }
}
