using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUIManager : MonoBehaviour
{
    //Calculator calculator;
    [SerializeField] private UIEffects uIEffects;

    public void Awake()
    {
        //calculator FindObjectOfType<GameManager>().gameObject.GetComponent<Calculator>(); 
    }

    public void SelectAction(VolleyPlayer selectedPlayer, int actionIndex)
    {
        //Action selectedAction = selectedPlayer.GetComponents<Action>()[actionIndex];

        //select action value and add it to game calcultator
        //calculator.AddActionValue(selectedAction.value);
        //uIEffects.ShowSelectedAction(selectedAction);
    }

    public void UnselectAction(VolleyPlayer selectedPlayer, int actionIndex)
    {
        //Action selectedAction = selectedPlayer.GetComponents<Action>()[actionIndex];

        //unselect action value and remove it from game calculator
        //calculator.AddActionValue(selectedAction.value);
        //uIEffects.ShowUnselectedAction(selectedAction);
    }

    public void SelectPlayerToReplace(VolleyPlayer selectedPlayer)
    {
        //select player from gamefield to be replaced
        uIEffects.ShowSelected(selectedPlayer);
    }

    public void SelectPlayerToPutOnField(VolleyPlayer selectedPlayer)
    {
        //select player from sidelines to put on game field
        uIEffects.ShowSelected(selectedPlayer);
    }

    public void SelectBlock(VolleyPlayer selectedPlayer)
    {
        //select a player form block
        uIEffects.ShowSelected(selectedPlayer);
    }


}
