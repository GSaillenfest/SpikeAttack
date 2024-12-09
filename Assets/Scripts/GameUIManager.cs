using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameUIManager : MonoBehaviour
{
    //Calculator calculator;
    [SerializeField]
    BlurControl blurControl;
    [SerializeField]
    FXCardManager cardFX;
    [SerializeField]
    TextMeshProUGUI powerTextVal;
    [SerializeField]
    TextMeshProUGUI previousPowerTextVal;
    [SerializeField]
    TextMeshProUGUI powerMalusText;
    [SerializeField]
    TextMeshProUGUI powerBonusText;
    [SerializeField]
    TextMeshProUGUI descriptionText;
    [SerializeField]
    TextMeshProUGUI orangeScore;
    [SerializeField]
    TextMeshProUGUI blueScore;
    [SerializeField]
    ScorePanelAnimationHandler scoredPanel;
    [SerializeField]
    Button bonusButtonOrange;
    [SerializeField]
    Button bonusButtonBlue;

    private Game game;

    public void Awake()
    {
        game = FindObjectOfType<Game>();
        ResetPowerBonusMalus();
        //calculator FindObjectOfType<GameManager>().gameObject.GetComponent<Calculator>(); 
    }

    public void SetBonusButton(Side side, bool value)
    {
        if (side == Side.Orange)
        {
            bonusButtonOrange.gameObject.SetActive(value);
        }
        else if (side == Side.Blue)
        {
            bonusButtonBlue.gameObject.SetActive(value);
        }
    }

    public void SetActionUnavailable(VolleyPlayer player, int actionIndex)
    {
        cardFX.ShowUnselectableAction(player, actionIndex);
    }

    public void SelectAction(VolleyPlayer selectedPlayer, int actionIndex)
    {
        cardFX.ShowSelected(selectedPlayer);
        cardFX.ShowSelectedAction(selectedPlayer, actionIndex);
    }

    public void DeselectAction(VolleyPlayer selectedPlayer, int actionIndex)
    {
        cardFX.ShowUnselected(selectedPlayer);
        //Action selectedAction = selectedPlayer.GetComponents<Action>()[actionIndex];
        cardFX.ShowUnselectedAction(selectedPlayer, actionIndex);
    }

    public void SelectPlayerBlock(VolleyPlayer playerCard, bool applyColor)
    {
        cardFX.ShowSelectedForBlock(playerCard, applyColor);
    }

    public void DeselectPlayerBlock(VolleyPlayer playerCard)
    {
        cardFX.ShowUnselectedForBlock(playerCard);
    }

    public void SelectPlayerToReplace(VolleyPlayer selectedPlayer)
    {
        //select player from gamefield to be replaced
        cardFX.ShowSelected(selectedPlayer);
    }

    public void SelectPlayerToPutOnField(VolleyPlayer selectedPlayer)
    {
        //select player from sidelines to put on game field
        cardFX.ShowSelected(selectedPlayer);
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

    internal void UpdatePreviousPowerMalusText(int malusValue = 0)
    {
        if (malusValue == 0)
            powerMalusText.text = "";
        else
            powerMalusText.text = "(-" + malusValue + ")";
    }

    internal void UpdatePowerBonusText(int bonusValue = 0)
    {
        if (bonusValue == 0)
            powerBonusText.text = "";
        else
            powerBonusText.text = "(+" + bonusValue + ")";
    }

    internal void ResetPowerBonusMalus()
    {
        UpdatePowerBonusText();
        UpdatePreviousPowerMalusText();
    }

    internal void DeactivateValidateButton()
    {
        ;
    }

    internal void DeselectCard(VolleyPlayer volleyPlayer)
    {
        cardFX.ShowUnselected(volleyPlayer);
    }

    internal void SetCardSelectable(VolleyPlayer volleyPlayer, bool isSelectable)
    {
        cardFX.ShowSelectable(volleyPlayer, isSelectable);
    }

    internal void ResetScaleAction(VolleyPlayer volleyPlayer, int actionIndex)
    {
        cardFX.ResetActionScaleOnly(volleyPlayer, actionIndex);
    }

    internal void SelectPlayerServe(VolleyPlayer playerCard)
    {
        cardFX.ShowSelectedForServe(playerCard);
    }

    internal void DeselectPlayerServe(VolleyPlayer playerCard)
    {
        cardFX.ShowUnselectedForServe(playerCard);
    }

    internal void UpdateScoreAnim(Side side)
    {
        CallBlurEffect(gameObject, 6);
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

    internal void SetActionAvailable(VolleyPlayer volleyPlayer, int actionIndex)
    {
        cardFX.ResetActionColorOnly(volleyPlayer, actionIndex);
    }

    internal void CallBlurEffect(GameObject parent, int orderInLayer = 0, GameObject[] exceptions = null)
    {
        blurControl.CallBlurEffect(parent, orderInLayer, exceptions);
    }

    // Must be called by animations on exit
    internal void EndTemporization()
    {
        game.EndTemporization();
    }

    internal void UpdateDescriptionText(string text)
    {
        descriptionText.text = text;
    }
}
