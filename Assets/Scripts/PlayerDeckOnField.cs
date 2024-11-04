using System;
using System.Collections.Generic;
using UnityEngine;


public class PlayerDeckOnField : MonoBehaviour
{
    [SerializeField]
    public GameObject[] deckSlots;

    List<VolleyPlayer> playersOnField = new();
    private int blockIndex = -1;

    // Start is called before the first frame update
    void Start()
    {
        //deckSlots = new GameObject[6];
        RefreshDeck();
    }

    public void RefreshDeck()
    {
        playersOnField.Clear();
        VolleyPlayer[] childList = gameObject.GetComponentsInChildren<VolleyPlayer>();
        foreach (VolleyPlayer child in childList)
        {
            playersOnField.Add(child);
        }
        SetSlotIndex();
    }

    public int GetActionValue(int slotIndex, int actionIndex)
    {
        return playersOnField[slotIndex].actionArr[actionIndex];
    }    
    
    public int GetBlockValue(int slotIndex)
    {
        return playersOnField[slotIndex].block;
    }

    public int GetBlockIndex()
    {
        return blockIndex;
    }

    public void RotatePlayerCards()
    {
        GameObject tempPlayerSlot = deckSlots[1];

        for (int i = deckSlots.Length - 1; i > 1; i--)
        {
            if (i == deckSlots.Length - 1)
            {
                //a la place du slot 1
                deckSlots[i].transform.GetChild(0).SetParent(deckSlots[1].transform, false);
            }
            else
            {
                //à la place du slot i + 1
                deckSlots[i].transform.GetChild(0).SetParent(deckSlots[i + 1].transform, false);
            }
        }
        // slot 2 = tempPlayerSlot
        tempPlayerSlot.transform.GetChild(0).SetParent(deckSlots[2].transform, false);

        RefreshDeck();
    }

    internal void SetAllSelectableCardAction(bool isSelectable)
    {
        foreach (VolleyPlayer player in playersOnField)
        {
            player.SetSelectable(isSelectable);
        }
    }

    internal void ValidateBlock(int slotIndex)
    {
        playersOnField[slotIndex].DeselectCard();
        playersOnField[slotIndex].DeselectBlock();
        blockIndex = slotIndex;
    }

    internal void ValidateActionCombo(int slotIndex, int actionIndex)
    {
        playersOnField[slotIndex].SetActionUnavailable(actionIndex);
        playersOnField[slotIndex].ResetScaleAction(actionIndex);
        playersOnField[slotIndex].DeselectCard();
        playersOnField[slotIndex].SetIsSelected(false);
        playersOnField[slotIndex].SetIsSelectedTwice(false);
    }

    internal void SetSelectableCardAction(int actionIndex, int[] selectedCardSlots)
    {
        if (actionIndex == 3)
        {
            foreach (VolleyPlayer player in playersOnField)
            {
                if (player.slotIndex == blockIndex) break;
                if (player.slotIndex == selectedCardSlots[actionIndex - 1])
                {
                    player.SetSelectable(true);
                }
                else
                {
                    player.SetSelectable(false);
                }
            }
        }
        else
        {
            foreach (VolleyPlayer player in playersOnField)
            {
                if (player.isActionAvailable[actionIndex] || (actionIndex > 0 && player.slotIndex == selectedCardSlots[actionIndex - 1]))
                {
                    player.SetSelectable(true);
                }
                else
                {
                    player.SetSelectable(false);
                }
            }
        };
    }

    public void SetSlotIndex()
    {
        for (int i = 0; i < playersOnField.Count; i++)
        {
            playersOnField[i].slotIndex = i;
        }
    }

    public void SetBlockPhase()
    {
        foreach (GameObject slot in deckSlots)
        {
            bool isSelectable = (slot.GetComponentInChildren<VolleyPlayer>().slotIndex >= 2 && slot.GetComponentInChildren<VolleyPlayer>().slotIndex <= 4);
            slot.GetComponentInChildren<VolleyPlayer>().SetSelectable(isSelectable);
        }
    }
}
