using TMPro;
using UnityEngine;

public class GameUIManager : MonoBehaviour
{
    //Calculator calculator;
    [SerializeField]
    private UIEffects uIEffects;
    [SerializeField]
    TextMeshProUGUI powerTextVal;
    [SerializeField]
    TextMeshProUGUI previousPowerTextVal;
    [SerializeField]
    TextMeshProUGUI powerMalusText;
    [SerializeField]
    TextMeshProUGUI orangeScore;
    [SerializeField]
    TextMeshProUGUI blueScore;
    [SerializeField]
    ScorePanelAnimationHandler scoredPanel;

    private Game game;

    public void Awake()
    {
        game = FindObjectOfType<Game>();
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
        uIEffects.ShowUnselectedAction(selectedPlayer, actionIndex);
    }

    public void SelectPlayerBlock(VolleyPlayer playerCard, bool applyColor)
    {
        uIEffects.ShowSelectedForBlock(playerCard, applyColor);
    }

    public void DeselectPlayerBlock(VolleyPlayer playerCard)
    {
        uIEffects.ShowUnselectedForBlock(playerCard);
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
    // deprecated
    /*    internal void ChangeCurrentTeam(TeamClass currentTeam)
        {
            uIEffects.ChangeTeamColorGradient(currentTeam);
        }*/

    internal void UpdatePowerText(int powerValue)
    {
        powerTextVal.text = powerValue.ToString();
    }

    internal void UpdatePreviousPowerText(int powerValue)
    {
        previousPowerTextVal.text = powerValue.ToString();
    }

    internal void UpdatePreviousPowerMalusText(int malusValue)
    {
        if (malusValue == 0)
            powerMalusText.text = "";
        else
            powerMalusText.text = "(-" + malusValue + ")";
    }

    internal void DeactivateValidateButton()
    {
        ;
    }

    internal void DeselectCard(VolleyPlayer volleyPlayer)
    {
        uIEffects.ShowUnselected(volleyPlayer);
    }

    internal void SetCardSelectable(VolleyPlayer volleyPlayer, bool isSelectable)
    {
        uIEffects.ShowSelectable(volleyPlayer, isSelectable);
    }

    internal void ResetScaleAction(VolleyPlayer volleyPlayer, int actionIndex)
    {
        uIEffects.ResetActionScaleOnly(volleyPlayer, actionIndex);
    }

    internal void SelectPlayerServe(VolleyPlayer playerCard)
    {
        uIEffects.ShowSelectedForServe(playerCard);
    }

    internal void DeselectPlayerServe(VolleyPlayer playerCard)
    {
        uIEffects.ShowUnselectedForServe(playerCard);
    }

    internal void UpdateScoreAnim(Side side)
    {
        if (side == Side.Orange)
        {
            scoredPanel.UpdateScoreText(side, "Orange Scored !");
        }
        else if (side == Side.Blue)
        {
            scoredPanel.UpdateScoreText(side, "Blue Scored !");
        }
        scoredPanel.StartAnim(side);
    }

    internal void UpdateScore()
    {
        int[] score = game.GetScore();
        orangeScore.text = score[0].ToString();
        blueScore.text = score[1].ToString();
    }

}
