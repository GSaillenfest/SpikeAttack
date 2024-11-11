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

    // State machine switching between phases of a game
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

    // Set only one playerCard for serve phase
    private void SetServePhase()
    {
        SetAllSelectableCardAction(currentTeam, false);
        SetAllSelectableCardAction(oppositeTeam, false);
        currentTeam.SetServePhase();
    }

    // Select and apply the serve value of the selected playerCard, update power
    private void SelectServeCard(VolleyPlayer selected)
    {
        isServeSelected = true;
        selected.SelectServe();
        actionArr[0] = currentTeam.GetServeValue();
        UpdatePowerValue();
        SetValidateButtonInteractable(true);
    }

    // End Serve phase and switch to next state
    private void ValidateServe()
    {
        isServeSelected = false;
        SetAllSelectableCardAction(currentTeam, false);
        currentTeam.ValidateServe();
        ChangePhase(Phase.BlockSelection);
    }

    // Switch to block resolution phase and call the resolve function
    private void SetBlockResolutionPhase()
    {
        currentPhase = Phase.BlockResolution;
        ResolveBlock();
    }

    // Compare the last opponent attack position and the block position
    // that has been selected at the end of the previous turn.
    // Switch state to Action phase
    private void ResolveBlock()
    {
        int previousBlockIndex = currentTeam.deckOnField.GetBlockIndex();

        // Return if no block has been previously selected
        if (previousBlockIndex == nonAttributed)
        {
            ChangePhase(Phase.Action);
            return;
        }

        // Compare both positions and values
        if (previousBlockIndex == 2 && (attackIndex == 4 || attackIndex == 5) ||
            previousBlockIndex == 3 && (attackIndex == 0 || attackIndex == 3) ||
            previousBlockIndex == 4 && (attackIndex == 1 || attackIndex == 2))
        {
            currentTeam.GetPlayerOnField(previousBlockIndex).SelectBlock(true);
            // End point if the block is greater or equal to the power, meaning that the current team has scored
            // If not, reduce the power by block value
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

    // Set playerCards selectable based on dig action availability
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

    // Select actions on card and add value to power
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

    // Set playerCards selectable based on action availability
    void SetSelectableCardAction()
    {
        currentTeam.SetSelectableCardAction(actionIndex, selectedCardSlots);
    }

    // Set all playerCards selectable or not
    void SetAllSelectableCardAction(TeamClass team, bool isSelectable)
    {
        team.SetAllSelectableCardField(isSelectable);
    }

    // End Action phase and perform a check 
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

    // End turn without selecting actions, make opponent score
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

    // Set front line playerCards selectable 
    void SetBlockSelectionPhase()
    {
        currentTeam.SetBlockPhase();
        SetValidateButtonInteractable(false);
    }

    // Select/Unselect block playerCard
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

    // Validate block position and memories it, end turn
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

    // Start a point, reset variables, Switch state to Serve phase
    void StartPoint(TeamClass team)
    {
        ResetVariables();
        ResetTeamStatus();
        gameUI.UpdatePowerText(powerValue);
        gameUI.UpdatePreviousPowerText(previousPowerValue);
        turn = 0;
        ChangePhase(Phase.Serve);
    }

    // Reset both teams status
    private void ResetTeamStatus()
    {
        currentTeam.ResetStatus();
        oppositeTeam.ResetStatus();
    }

    // Start a new turn, switch state to BlockResolution
    public void StartTurn(TeamClass team)
    {
        turn++;
        ChangePhase(Phase.BlockResolution);
    }

    // End normal turn and switch team then call start turn
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

    // Compare current power and previous turn power
    internal void CheckWinningConditions(int powerValue, int previousPowerValue)
    {

        if (powerValue > previousPowerValue)
        {
            ChangePhase(Phase.BlockSelection);
        }
        else EndPoint();
    }

    // End point, switch to BlockSelection phase, launch animation
    private void EndPoint()
    {
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

    // TODO: To implement
    public void SelectCardEffect()
    {
        /*if (isSelected)
        card.getComponent<Effect>().Activate();*/
    }

    // Select fucntion called by playerCard selection based on current state
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

    // Add button cooldown on card selection to avoid interferences with animations
    // (could be handled by animator)
    IEnumerator AddBtnCooldown()
    {
        Phase initialPhase = currentPhase;
        currentPhase = Phase.Inactive;
        yield return new WaitForSeconds(cooldownDuration);
        currentPhase = initialPhase;
    }

    // Select function called by validate button based on current state
    public void HandleValidateButtonFunction()
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
