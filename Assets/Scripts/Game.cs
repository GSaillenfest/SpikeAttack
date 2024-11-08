using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    [SerializeField]
    private TeamClass team1;
    [SerializeField]
    private TeamClass team2;
    [SerializeField]
    GameUIManager gameUI;
    [SerializeField]
    Button startBtn;
    [SerializeField]
    Button validateBtn;
    [SerializeField]
    private Button endTurnBtn;
    [SerializeField]
    float cooldownDuration;

    GameManager gameManager;
    private int turn;
    private int[] selectedCardSlots;
    private int[] actionArr;
    private TeamClass currentTeam;
    private TeamClass oppositeTeam;
    private bool isGameStarted;
    // to be refactored
    private bool isServeSelected;
    private int powerValue = 0;
    private int previousPowerValue = 0;
    private int attackIndex;
    private const int nonAttributed = -1;

    public int actionIndex = 0;
    private Phase currentPhase;
    private int orangeSideScore = 0;
    private int blueSideScore = 0;

    public Game(TeamClass t1, TeamClass t2)
    {
        team1 = t1;
        team2 = t2;
        turn = 0;
    }

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        team1 = gameManager.teams[0];
        team2 = gameManager.teams[1];
        selectedCardSlots = new int[3];
        EmptySelectedCardSlots();
        actionArr = new int[3];
        attackIndex = nonAttributed;
        currentPhase = Phase.Inactive;
        cooldownDuration = 0.6f;
        SetEndTurnBtnInteractable(false);
        SetValidateButtonInteractable(false);
    }

    void ChangePhase(Phase phase)
    {
        currentPhase = phase;
        switch (currentPhase)
        {
            case Phase.TeamSelection:
                break;
            case Phase.BlockSelection:
                SetBlockSelectionPhase();
                break;
            case Phase.BlockResolution:
                SetBlockResolutionPhase();
                break;
            case Phase.Action:
                SetActionPhase();
                break;
            case Phase.Replacement:
                break;
            case Phase.Serve:
                SetServePhase();
                break;
            case Phase.Inactive:
                break;
            default:
                break;
        }
    }

    private void SetServePhase()
    {
        SetAllSelectableCardAction(currentTeam, false);
        SetAllSelectableCardAction(oppositeTeam, false);
        currentTeam.SetServePhase();
    }

    private void SelectServeCard(VolleyPlayer selected)
    {
        isServeSelected = true;
        selected.SelectServe();
        actionArr[0] = currentTeam.GetServeValue();
        UpdatePowerValue();
        SetValidateButtonInteractable(true);
    }

    private void ValidateServe()
    {
        isServeSelected = false;
        SetAllSelectableCardAction(currentTeam, false);
        currentTeam.ValidateServe();
        ChangePhase(Phase.BlockSelection);
    }

    private void SetBlockResolutionPhase()
    {
        currentPhase = Phase.BlockResolution;
        ResolveBlock();
    }

    private void ResolveBlock()
    {
        int previousBlockIndex = currentTeam.deckOnField.GetBlockIndex();
        if (previousBlockIndex == nonAttributed)
        {
            ChangePhase(Phase.Action);
            return;
        }
        if (previousBlockIndex == 2 && (attackIndex == 4 || attackIndex == 5) ||
            previousBlockIndex == 3 && (attackIndex == 0 || attackIndex == 3) ||
            previousBlockIndex == 4 && (attackIndex == 1 || attackIndex == 2))
        {
            currentTeam.GetPlayerOnField(previousBlockIndex).SelectBlock(true);
            if (currentTeam.deckOnField.GetBlockValue(previousBlockIndex) >= previousPowerValue)
            {
                gameManager.EndPoint();
            }
            else
            {
                previousPowerValue -= currentTeam.deckOnField.GetBlockValue(previousBlockIndex);
                gameUI.UpdatePreviousPowerText(previousPowerValue);
                gameUI.UpdatePreviousPowerMalusText(currentTeam.deckOnField.GetBlockValue(previousBlockIndex));
                // TODO: Add UI FX
                ChangePhase(Phase.Action);
            }
        }
        else
        {
            currentTeam.GetPlayerOnField(previousBlockIndex).SelectBlock(false);
            ChangePhase(Phase.Action);
        }
    }

    private void SetActionPhase()
    {
        EmptySelectedCardSlots();
        actionArr[0] = actionArr[1] = actionArr[2] = 0;
        actionIndex = 0;
        SetSelectableCardAction();
        SetAllSelectableCardAction(oppositeTeam, false);
        SetValidateButtonInteractable(false);
        SetEndTurnBtnInteractable(true);
    }

    public void SelectAction(VolleyPlayer selected)
    {
        if (actionIndex == 0)
        {
            actionArr[actionIndex] = selected.actionArr[actionIndex];
            selectedCardSlots[actionIndex] = selected.slotIndex;
            selected.SelectActionAnimation(actionIndex);
            selected.SetIsSelected(true);
            actionIndex++;
            SetSelectableCardAction();
        }
        else if (actionIndex < 3)
        {
            if (selectedCardSlots[actionIndex - 1] == selected.slotIndex)
            {
                selectedCardSlots[actionIndex - 1] = nonAttributed;
                actionArr[actionIndex - 1] = 0;
                selected.DeselectActionAnimation(actionIndex - 1);
                selected.SetIsSelected(false);
                actionIndex--;
                SetSelectableCardAction();
            }
            else
            {
                actionArr[actionIndex] = selected.actionArr[actionIndex];
                selectedCardSlots[actionIndex] = selected.slotIndex;
                selected.SelectActionAnimation(actionIndex);

                if (!selected.isSelected)
                    selected.SetIsSelected(true);
                else selected.SetIsSelectedTwice(true);

                actionIndex++;
                SetSelectableCardAction();
            }
        }
        else if (actionIndex == 3)
        {
            if (selectedCardSlots[actionIndex - 1] == selected.slotIndex)
            {
                selectedCardSlots[actionIndex - 1] = nonAttributed;
                actionArr[actionIndex - 1] = 0;
                selected.DeselectActionAnimation(actionIndex - 1);
                if (selected.isSelectedTwice)
                    selected.SetIsSelectedTwice(false);
                else selected.SetIsSelected(false);
                SetEndTurnBtnInteractable(true);
                SetValidateButtonInteractable(false);
                gameUI.DeactivateValidateButton();
                actionIndex--;
                SetSelectableCardAction();
            }
        }

        if (actionIndex == 3)
        {
            SetEndTurnBtnInteractable(false);
            SetValidateButtonInteractable(true);
        }

        UpdatePowerValue();
    }

    void SetSelectableCardAction()
    {
        currentTeam.SetSelectableCardAction(actionIndex, selectedCardSlots);
    }

    void SetAllSelectableCardAction(TeamClass team, bool isSelectable)
    {
        team.SetAllSelectableCardField(isSelectable);
    }

    void ValidateActions()
    {
        SetAllSelectableCardAction(currentTeam, false);
        for (int i = 0; i < selectedCardSlots.Length; i++)
        {
            currentTeam.ValidateActionCombo(selectedCardSlots[i], i);
        }
        attackIndex = selectedCardSlots[2];
        EmptySelectedCardSlots();
        SetValidateButtonInteractable(false);
        CheckWinningConditions(powerValue, previousPowerValue);
    }

    public void EndTurnOnClick()
    {
        SetAllSelectableCardAction(currentTeam, false);
        for (int i = 0; i < actionIndex; i++)
        {
            currentTeam.GetPlayerOnField(selectedCardSlots[i]).SetIsSelected(false);
            currentTeam.GetPlayerOnField(selectedCardSlots[i]).SetIsSelectedTwice(false);
            currentTeam.GetPlayerOnField(selectedCardSlots[i]).DeselectActionAnimation(i);
        }
        EmptySelectedCardSlots();
        SetValidateButtonInteractable(false);
        SetEndTurnBtnInteractable(false);
        powerValue = 0;
        // TODO: Deselect Card with effect
        CheckWinningConditions(powerValue, previousPowerValue);
    }

    void SetBlockSelectionPhase()
    {
        currentTeam.SetBlockPhase();
        SetValidateButtonInteractable(false);
    }

    public void SelectBlockCard(VolleyPlayer player)
    {
        if (selectedCardSlots[0] == nonAttributed)
        {
            selectedCardSlots[0] = player.slotIndex;
            SetAllSelectableCardAction(currentTeam, false);
            player.SetSelectable(true);
            player.SelectBlock(true);
            SetValidateButtonInteractable(true);
        }
        else
        {
            player.DeselectBlock();
            selectedCardSlots[0] = nonAttributed;
            SetBlockSelectionPhase();
        }
    }

    void ValidateBlockSelection()
    {
        SetAllSelectableCardAction(currentTeam, false);
        currentTeam.ValidateBlock(selectedCardSlots[0]);
        EndCurrentTurn();
    }


    // Start a regular game (Called by UI Button for now)
    public void StartGame(TeamClass team)
    {
        // avoid two startgame clicks
        startBtn.interactable = false;
        startBtn.gameObject.SetActive(false);
        currentTeam = team;
        oppositeTeam = GetOppositeTeam(currentTeam);
        StartPoint(team);
    }

    void StartPoint(TeamClass team)
    {
        ResetVariables();
        ResetTeamStatus();
        gameUI.UpdatePowerText(powerValue);
        gameUI.UpdatePreviousPowerText(previousPowerValue);
        turn = 0;
        ChangePhase(Phase.Serve);
    }

    private void ResetTeamStatus()
    {
        currentTeam.ResetStatus();
        oppositeTeam.ResetStatus();
    }

    public void StartTurn(TeamClass team)
    {
        turn++;
        ChangePhase(Phase.BlockResolution);
    }

    void EndCurrentTurn()
    {
        Debug.Log("No winning side : End Turn");
        previousPowerValue = powerValue;
        powerValue = 0;
        gameUI.UpdatePowerText(powerValue);
        gameUI.UpdatePreviousPowerText(previousPowerValue);
        gameUI.UpdatePreviousPowerMalusText();
        gameUI.UpdatePowerBonusText();
        SwitchTeam();
        StartTurn(currentTeam);
    }

    private void EmptySelectedCardSlots()
    {
        selectedCardSlots[0] = nonAttributed;
        selectedCardSlots[1] = nonAttributed;
        selectedCardSlots[2] = nonAttributed;
    }

    internal void SetCurrentTeam(TeamClass currentTeam)
    {
        this.currentTeam = currentTeam;
        oppositeTeam = GetOppositeTeam(currentTeam);
    }

    TeamClass GetOppositeTeam(TeamClass team)
    {
        return team == team1 ? team2 : team1;
    }

    private void SetValidateButtonInteractable(bool isInteractable)
    {
        validateBtn.interactable = isInteractable;
    }

    private void SetEndTurnBtnInteractable(bool isInteractable)
    {
        endTurnBtn.interactable = isInteractable;
    }

    private void UpdatePowerValue()
    {
        powerValue = 0;
        for (int i = 0; i < actionArr.Length; i++)
        {
            powerValue += actionArr[i];
        }
        gameUI.UpdatePowerText(powerValue);
    }

    public void SelectCardEffect()
    {
        /*if (isSelected)
        card.getComponent<Effect>().Activate();*/
    }

    public void HandleCardButtonFunction(VolleyPlayer player)
    {
        switch (currentPhase)
        {
            case Phase.TeamSelection:
                break;
            case Phase.BlockSelection:
                SelectBlockCard(player);
                break;
            case Phase.BlockResolution:
                break;
            case Phase.Action:
                SelectAction(player);
                break;
            case Phase.Replacement:
                break;
            case Phase.Serve:
                if (!isServeSelected) SelectServeCard(player);
                break;
            case Phase.Inactive:
                return;
            default:
                break;
        }

        StartCoroutine(AddBtnCooldown());
    }

    IEnumerator AddBtnCooldown()
    {
        Phase initialPhase = currentPhase;
        currentPhase = Phase.Inactive;
        yield return new WaitForSeconds(cooldownDuration);
        currentPhase = initialPhase;
    }

    public void SelectValidateButtonFunction()
    {
        switch (currentPhase)
        {
            case Phase.TeamSelection:
                break;
            case Phase.BlockSelection:
                ValidateBlockSelection();
                break;
            case Phase.BlockResolution:
                break;
            case Phase.Action:
                ValidateActions();
                break;
            case Phase.Replacement:
                break;
            case Phase.Serve:
                ValidateServe();
                break;
            default:
                break;
        }
    }

    internal void CheckWinningConditions(int powerValue, int previousPowerValue)
    {

        if (powerValue > previousPowerValue)
        {
            ChangePhase(Phase.BlockSelection);
        }
        else EndPoint();
    }

    private void EndPoint()
    {
        // TODO: add btn to pass turn with double check
        Debug.Log("End point, reset values");
        ChangePhase(Phase.BlockSelection);
        if (currentTeam == team1)
        {
            blueSideScore++;
            gameUI.UpdateScoreAnim(Side.Blue);
        }
        else
        {
            orangeSideScore++;
            gameUI.UpdateScoreAnim(Side.Orange);
        }
        // TODO: Add Rotation and Replacement phases
        SwitchTeam();
        StartPoint(currentTeam);
    }

    internal void ResetVariables()
    {
        powerValue = previousPowerValue = 0;
    }

    void SwitchTeam()
    {
        currentTeam = currentTeam == team1 ? team2 : team1;
        oppositeTeam = GetOppositeTeam(currentTeam);
    }

    internal int[] GetScore()
    {
        return new int[] { orangeSideScore, blueSideScore};
    }
}
