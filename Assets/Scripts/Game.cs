using System.Collections;
using System.Collections.Generic;
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
    private Button noBlockButton;
    [SerializeField]
    float cooldownDuration;
    [SerializeField]
    int allowedRemplacementNumber = 2;
    [SerializeField]
    private EffectManager effectManager;

    int replacement;
    GameManager gameManager;
    private int turn;
    private int[] selectedCardSlots;
    private int[] actionArr;
    //private List<BonusCard> selectedBonusCards = new();
    private TeamClass currentTeam;
    private Side currentSide;
    private TeamClass oppositeTeam;
    private bool isGameStart;
    // to be refactored
    private bool isServeSelected;
    private int powerValue = 0;
    private int bonusPowerValue = 0;
    public int BonusPowerValue { get => bonusPowerValue; set => SetBonusPowerValue(value); }
    private void SetBonusPowerValue(int value)
    {
        bonusPowerValue = value;
        gameUI.UpdatePowerBonusText(bonusPowerValue);
    }
    private int previousPowerValue = 0;
    private int attackIndex;
    private const int nonAttributed = -1;
    private int replacementNumber = 0;
    public int actionIndex = 0;
    public Phase currentPhase;
    private int orangeSideScore = 0;
    private int blueSideScore = 0;
    private Phase tempPhase;
    private List<BonusCard> bonusCardList;

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
        selectedCardSlots = new int[6];

        EmptySelectedCardSlots();
        actionArr = new int[3];
        attackIndex = nonAttributed;
        replacementNumber = 0;
        replacement = allowedRemplacementNumber;
        currentPhase = Phase.Inactive;
        cooldownDuration = 0.6f;
        SetEndTurnBtnInteractable(false);
        SetValidateButtonInteractable(false);
        bonusCardList = new();
    }

    // State machine switching between phases of a game
    public void ChangePhase(Phase phase)
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
                SetPreReplacementPhase();
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

    internal void Temporize(Phase nextPhase)
    {
        tempPhase = nextPhase;
        ChangePhase(Phase.Inactive);
    }

    internal void EndTemporization()
    {
        ChangePhase(tempPhase);
    }

    private void SetPreReplacementPhase()
    {
        currentTeam.RotateFieldCards();
        oppositeTeam.RotateFieldCards();
        SetReplacementPhase();
    }

    private void SetReplacementPhase()
    {
        gameUI.UpdateDescriptionText("Replace 2 players");
        SwitchTeam();
        EmptySelectedCardSlots();

        // show SideDeck and Set card Selectable
        currentTeam.ShowSidelines(true);
        SetAllSelectableCardOnField(currentTeam, true);
        SetAllSelectableCardOnSide(currentTeam, true);
        CallBlurReplacement();
    }

    private void CallBlurReplacement()
    {
        GameObject[] exceptionsBlur = new GameObject[currentTeam.deckOnSide.playersOnSidelines.Count];
        for (int i = 0; i < exceptionsBlur.Length; i++)
        {
            exceptionsBlur[i] = currentTeam.deckOnSide.playersOnSidelines[i].gameObject;
        }
        gameUI.CallBlurEffect(currentTeam.deckOnSide.gameObject, 0, exceptionsBlur);
    }

    private void ReplaceCard(VolleyPlayer selectedCard)
    {

        // When a card is selected
        if (selectedCardSlots[0] == nonAttributed)
        {
            selectedCardSlots[0] = selectedCard.slotIndex;

            //Check if is a card on field
            if (selectedCard.slotIndex < 6)
            {
                // Check if is a libero
                if (selectedCard.isLibero)
                {
                    // Set all field cards unselectable
                    SetAllSelectableCardOnField(currentTeam, false);
                    // Set non libero cards on sidelines unselectable
                    SetAllSelectableCardOnSide(currentTeam, false);
                    currentTeam.GetPlayerBySlotIndex(6).SetSelectable(true);
                }
                else
                {
                    // Set all field cards unselectable
                    SetAllSelectableCardOnField(currentTeam, false);
                    // Set sidelines libero card unselectable
                    currentTeam.GetPlayerBySlotIndex(6).SetSelectable(false);
                }
            }
            else
            {
                // Check if is libero
                if (selectedCard.isLibero)
                {
                    // Set all sidelines cards unselectable
                    SetAllSelectableCardOnSide(currentTeam, false);
                    // Set all non libero field cards unselectable
                    SetAllSelectableCardOnField(currentTeam, false);
                    currentTeam.GetPlayerBySlotIndex(0).SetSelectable(true);
                }
                else
                {
                    // Set all side cards unselectable
                    SetAllSelectableCardOnSide(currentTeam, false);
                    // Set libero field card unselectable
                    currentTeam.GetPlayerBySlotIndex(0).SetSelectable(false);
                }
            }
            if (selectedCardSlots[2] != nonAttributed)
            {
                currentTeam.GetPlayerBySlotIndex(selectedCardSlots[2]).SetSelectable(false);
                currentTeam.GetPlayerBySlotIndex(selectedCardSlots[3]).SetSelectable(false);
            }
            selectedCard.SetSelectable(true);
        }
        else if (selectedCard.slotIndex == selectedCardSlots[0])
        {
            // Reset all cards selectable but the previously replaced
            SetAllSelectableCardOnField(currentTeam, true);
            SetAllSelectableCardOnSide(currentTeam, true);
            if (selectedCardSlots[2] != nonAttributed)
            {
                currentTeam.GetPlayerBySlotIndex(selectedCardSlots[2]).SetSelectable(false);
                currentTeam.GetPlayerBySlotIndex(selectedCardSlots[3]).SetSelectable(false);
            }
            selectedCardSlots[0] = nonAttributed;
        }
        else
        {
            selectedCardSlots[1] = selectedCard.slotIndex;
            CallBlurReplacement();
            currentTeam.SwitchTwoCardPlayerSlot(selectedCardSlots[0], selectedCardSlots[1]);
            CallBlurReplacement();
            SetAllSelectableCardOnField(currentTeam, true);
            SetAllSelectableCardOnSide(currentTeam, true);
            // Keep previously replaced cards in memory
            selectedCardSlots[2] = selectedCardSlots[0];
            selectedCardSlots[3] = selectedCardSlots[1];
            selectedCardSlots[0] = nonAttributed;
            selectedCardSlots[1] = nonAttributed;
            // Set previously replaced cards unselectable
            currentTeam.GetPlayerBySlotIndex(selectedCardSlots[2]).SetSelectable(false);
            currentTeam.GetPlayerBySlotIndex(selectedCardSlots[3]).SetSelectable(false);
            // count down from number of allowed replacement
            replacement--;
            gameUI.UpdateDescriptionText("Replace 1 player");
        }

        if (replacement == 0)
        {
            SetValidateButtonInteractable(true);
            SetAllSelectableCardOnField(currentTeam, false);
            SetAllSelectableCardOnSide(currentTeam, false);
            replacement = allowedRemplacementNumber;
        }

    }

    private void SetAllSelectableCardOnSide(TeamClass currentTeam, bool isSelectable)
    {
        currentTeam.SetAllSelectableCardOnSide(isSelectable);
    }

    private void ValidateReplacement()
    {
        currentTeam.ShowSidelines(false);
        gameUI.CallBlurEffect(currentTeam.deckOnSide.gameObject, 0, currentTeam.deckOnSide.deckSlots);
        replacementNumber++;
        EmptySelectedCardSlots();

        // if both team made their replacement then start new turn
        if (replacementNumber == 2)
        {
            replacementNumber = 0;
            SwitchTeam();
            ChangePhase(Phase.Serve);
        }
        else // else change replacement side
        {
            SetReplacementPhase();
        }
    }

    // Set only one playerCard for serve phase
    private void SetServePhase()
    {
        gameUI.UpdateDescriptionText("Select the server then click on Validate");
        EmptySelectedCardSlots();
        SetValidateButtonInteractable(false);
        SetAllSelectableCardOnField(currentTeam, false);
        SetAllSelectableCardOnField(oppositeTeam, false);
        currentTeam.SetServePhase();
        SetBonusButton();
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
        SetAllSelectableCardOnField(currentTeam, false);
        currentTeam.ValidateServe();
        ChangePhase(Phase.BlockSelection);
    }

    // Switch to block resolution phase and call the resolve function
    private void SetBlockResolutionPhase()
    {
        // ////////////////// TODO: Check if necessary 
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
            currentTeam.GetPlayerBySlotIndex(previousBlockIndex).SelectBlock(true);
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
            currentTeam.GetPlayerBySlotIndex(previousBlockIndex).SelectBlock(false);
            ChangePhase(Phase.Action);
        }
    }

    // Set playerCards selectable based on dig action availability
    private void SetActionPhase()
    {
        EmptySelectedCardSlots();
        actionArr[0] = actionArr[1] = actionArr[2] = 0;
        actionIndex = 0;
        SetSelectableCardByAction();
        SetAllSelectableCardOnField(oppositeTeam, false);
        SetValidateButtonInteractable(false);
        SetEndTurnBtnInteractable(true);
        gameUI.UpdateDescriptionText("Select Dig, Pass and Attack");
        SetBonusButton();
    }

    void SetBonusButton()
    {
        if (currentTeam == team1)
        {
            gameUI.SetBonusButton(Side.Orange, true);
            gameUI.SetBonusButton(Side.Blue, false);
        }
        else
        {
            gameUI.SetBonusButton(Side.Blue, true);
            gameUI.SetBonusButton(Side.Orange, false);
        }
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
            SetSelectableCardByAction();
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
                SetSelectableCardByAction();
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
                SetSelectableCardByAction();
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
                SetSelectableCardByAction();
            }
        }

        if (actionIndex == 3)
        {
            SetEndTurnBtnInteractable(false);
            SetValidateButtonInteractable(true);
        }

        UpdatePowerValue();
    }

    // End Action phase and perform a check 
    void ValidateActions()
    {
        SetAllSelectableCardOnField(currentTeam, false);
        for (int i = 0; i < 3; i++)
        {
            currentTeam.ValidateActionCombo(selectedCardSlots[i], i);
        }
        attackIndex = selectedCardSlots[2];
        EmptySelectedCardSlots();
        SetValidateButtonInteractable(false);
        powerValue += bonusPowerValue;
        CheckWinningConditions(powerValue, previousPowerValue);
    }

    // End turn without selecting actions, make opponent score
    public void EndTurnOnClick()
    {
        SetAllSelectableCardOnField(currentTeam, false);
        for (int i = 0; i < actionIndex; i++)
        {
            currentTeam.GetPlayerBySlotIndex(selectedCardSlots[i]).SetIsSelected(false);
            currentTeam.GetPlayerBySlotIndex(selectedCardSlots[i]).SetIsSelectedTwice(false);
            currentTeam.GetPlayerBySlotIndex(selectedCardSlots[i]).DeselectActionAnimation(i);
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
        gameUI.UpdateDescriptionText("Select a block then click on Validate");
        currentTeam.SetBlockSelectionPhase();
        SetValidateButtonInteractable(false);
        noBlockButton.gameObject.SetActive(true);
        SetNoBlockButton(true);
    }

    // Select/Unselect block playerCard
    void SelectBlockCard(VolleyPlayer player)
    {
        if (player == null)
        {
            if (selectedCardSlots[0] != nonAttributed)
            {
                SetNoBlockButton(true);
                player.DeselectBlock();
                selectedCardSlots[0] = nonAttributed;
                SetBlockSelectionPhase();
            }

            selectedCardSlots[0] = nonAttributed;
            SetValidateButtonInteractable(true);
            SetNoBlockButton(false);
            return;
        }

        if (selectedCardSlots[0] == nonAttributed)
        {
            SetNoBlockButton(false);
            selectedCardSlots[0] = player.slotIndex;
            SetAllSelectableCardOnField(currentTeam, false);
            player.SetSelectable(true);
            player.SelectBlock(true);
            SetValidateButtonInteractable(true);
        }
        else
        {
            SetNoBlockButton(true);
            player.DeselectBlock();
            selectedCardSlots[0] = nonAttributed;
            SetBlockSelectionPhase();
        }
    }

    private void SetNoBlockButton(bool isInteractable)
    {
        noBlockButton.interactable = isInteractable;
    }

    public void SelectNoBlock()
    {
        SelectBlockCard(null);
    }

    // Validate block position and memories it, end turn
    void ValidateBlockSelection()
    {
        noBlockButton.gameObject.SetActive(false);
        SetAllSelectableCardOnField(currentTeam, false);
        if (selectedCardSlots[0] != nonAttributed)
            currentTeam.ValidateBlock(selectedCardSlots[0]);
        EndCurrentTurn();
    }

    // Start a regular game (Called by UI Button for now)
    public void StartGame(TeamClass team)
    {
        // avoid two startgame clicks
        startBtn.interactable = false;
        startBtn.gameObject.SetActive(false);

        isGameStart = true;
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
        if (isGameStart)
        {
            isGameStart = false;
            ChangePhase(Phase.Serve);
        }
        // else ChangePhase(Phase.Replacement);
        // Next phase must be triggered by the end of animation
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
        // ChangePhase(Phase.BlockSelection);
        Temporize(Phase.Replacement);
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
        StartPoint(currentTeam);
    }



    // Set playerCards selectable based on action availability
    void SetSelectableCardByAction()
    {
        currentTeam.SetSelectableCardAction(actionIndex, selectedCardSlots);
    }

    // Set all playerCards selectable or not
    void SetAllSelectableCardOnField(TeamClass team, bool isSelectable)
    {
        team.SetAllSelectableCardField(isSelectable);
    }

    private void EmptySelectedCardSlots()
    {
        for (int i = 0; i < selectedCardSlots.Length; i++)
        {
            selectedCardSlots[i] = nonAttributed;
        }
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

    public void UpdatePowerValue()
    {
        Debug.Log("test");
        powerValue = 0;
        for (int i = 0; i < actionArr.Length; i++)
        {
            powerValue += actionArr[i];
        }
        gameUI.UpdatePowerBonusText(bonusPowerValue);
        gameUI.UpdatePowerText(powerValue);
    }

    public void OnBonusSelection(BonusCard card)
    {
        if (bonusCardList.Contains(card))
        {
            bonusCardList.Remove(card);
            DeselectBonusCard(card);
        }
        else
        {
            bonusCardList.Add(card);
            SelectBonusCard(card);
        }
    }

    public void SelectBonusCard(BonusCard card)
    {
        // TODO gameUI.CardSelection
        effectManager.AddEffectToList(card.cardEffect);
    }

    void DeselectBonusCard(BonusCard card)
    {
        effectManager.RemoveEffectFromList(card.cardEffect);
    }

    // Select function called by playerCard selection based on current state
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
                ReplaceCard(player);
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
                gameUI.HideBonusPanel(currentSide);
                ValidateBlockSelection();
                break;
            case Phase.BlockResolution:
                break;
            case Phase.Action:
                gameUI.HideBonusPanel(currentSide);
                ValidateActions();
                break;
            case Phase.Replacement:
                gameUI.HideBonusPanel(currentSide);
                ValidateReplacement();
                break;
            case Phase.Serve:
                gameUI.HideBonusPanel(currentSide);
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
        currentSide = currentTeam == team1 ? Side.Orange : Side.Blue;
        oppositeTeam = GetOppositeTeam(currentTeam);
        effectManager.ClearEffectList();
    }

    internal int[] GetScore()
    {
        return new int[] { orangeSideScore, blueSideScore };
    }


}
