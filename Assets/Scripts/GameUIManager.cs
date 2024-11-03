using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameUIManager : MonoBehaviour
{
    //Calculator calculator;
    [SerializeField] 
    private UIEffects uIEffects;
    [SerializeField]
    TextMeshProUGUI powerText;

    public void Awake()
    {
        //calculator FindObjectOfType<GameManager>().gameObject.GetComponent<Calculator>(); 
    }

    public void SetActionUnavailable(VolleyPlayer player, int actionIndex)
    {
        uIEffects.ShowUnselectableAction(player, actionIndex);
    }

    public void SelectAction(VolleyPlayer selectedPlayer, int actionIndex)
    {
        uIEffects.ShowSelected(selectedPlayer);
        uIEffects.ShowSelectedAction(selectedPlayer, actionIndex);
    }

    public void DeselectAction(VolleyPlayer selectedPlayer, int actionIndex)
    {
        uIEffects.ShowUnselected(selectedPlayer);
        //Action selectedAction = selectedPlayer.GetComponents<Action>()[actionIndex];

        //unselect action value and remove it from game calculator
        //calculator.AddActionValue(selectedAction.value);
        uIEffects.ShowUnselectedAction(selectedPlayer, actionIndex);
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

    internal void ChangeCurrentTeam(TeamClass currentTeam)
    {
        uIEffects.ChangeTeamColorGradient(currentTeam);
    }

    internal void UpdatePowerText(object powerValue)
    {
        powerText.text = powerValue.ToString();
    }

    public void SelectBlock(VolleyPlayer selectedPlayer)
    {
        //select a player form block
        uIEffects.ShowSelected(selectedPlayer);
    }

    internal void DeactivateValidateButton()
    {
        ;
    }

    internal void DeselectCard(VolleyPlayer volleyPlayer)
    {
        uIEffects.ShowUnselected(volleyPlayer);
    }
}
