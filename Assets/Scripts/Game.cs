using System;
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

    GameManager gameManager;
    private int turn;
    private int[] selectedCardSlots;
    private int[] actionArr;
    private TeamClass currentTeam;
    private TeamClass nonPlayingTeam;
    private bool isGameStarted;
    private int powerValue = 0;
    private int previousPowerValue = 0;
    private int attackIndex;
    private const int nonAttributed = -1;

    public int actionIndex = 0;
    private Phase currentPhase;

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
                SetActionPhase(currentTeam);
                break;
            case Phase.Replacement:
                break;
            case Phase.Serve:
                SetServePhase();
                break;
            default:
                break;
        }
    }

    private void SetServePhase()
    {
        SetAllSelectableCardAction(currentTeam, false);
        SetAllSelectableCardAction(nonPlayingTeam, false);
        currentTeam.SetServePhase();
    }

    private void SelectServeCard(VolleyPlayer selected)
    {
        selected.SelectServe();
        SetValidateButtonInteractable(true);
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
            Debug.Log("No block");
            return;
        }
        if (previousBlockIndex == 2 && (attackIndex == 4 || attackIndex == 5) ||
            previousBlockIndex == 3 && (attackIndex == 0 || attackIndex == 3) ||
            previousBlockIndex == 4 && (attackIndex == 1 || attackIndex == 2))
        {
            if (currentTeam.deckOnField.GetBlockValue(previousBlockIndex) >= previousPowerValue)
            {
                Debug.Log("Block succeed : Point");
                gameManager.EndPoint();
            }
            else
            {
                Debug.Log("Block succeed : Previous power value is reduced");
                previousPowerValue -= currentTeam.deckOnField.GetBlockValue(previousBlockIndex);
                gameUI.UpdatePreviousPowerText(previousPowerValue);
                gameUI.UpdatePreviousPowerMalusText(currentTeam.deckOnField.GetBlockValue(previousBlockIndex));
                // Add UI FX
                ChangePhase(Phase.Action);
            }
        }
        else
        {
            Debug.Log("Block failed");
            ChangePhase(Phase.Action);
        }
    }

    private void SetActionPhase(TeamClass team)
    {
        EmptySelectedCardSlots();
        actionArr[0] = actionArr[1] = actionArr[2] = 0;
        actionIndex = 0;
        currentTeam = team;
        nonPlayingTeam = GetOppositeTeam(currentTeam);
        SetSelectableCardAction();
        SetAllSelectableCardAction(nonPlayingTeam, false);
        SetValidateButtonInteractable(false);
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
                SetValidateButtonInteractable(false);
                gameUI.DeactivateValidateButton();
                actionIndex--;
                SetSelectableCardAction();
            }
        }

        if (actionIndex == 3)
        {
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
        ChangePhase(Phase.BlockSelection);
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
            player.SelectBlock();
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
        EndTurn();
    }


    // Start a regular game (Called by UI Button for now)
    public void StartGame(int team = 1)
    {
        // avoid two startgame clicks
        startBtn.interactable = false;
        currentTeam = team == 1 ? team1 : team2;
        gameUI.UpdatePowerText(powerValue);
        gameUI.UpdatePreviousPowerText(previousPowerValue);
        turn = 0;
        ChangePhase(Phase.Serve);
    }

    public void StartTurn()
    {
        turn++;
        // to be redifined ////////////
        ChangePhase(Phase.BlockResolution);
    }

    void EndTurn()
    {
        previousPowerValue = powerValue;
        powerValue = 0;
        gameUI.UpdatePowerText(powerValue);
        gameUI.UpdatePreviousPowerText(previousPowerValue);
        gameUI.UpdatePreviousPowerMalusText(0);
        gameManager.EndTurn();
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
        nonPlayingTeam = GetOppositeTeam(currentTeam);
    }

    TeamClass GetOppositeTeam(TeamClass team)
    {
        return team == team1 ? team2 : team1;
    }

    private void SetValidateButtonInteractable(bool isInteractable)
    {
        validateBtn.interactable = isInteractable;
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

    public void SelectCardButtonFunction(VolleyPlayer player)
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
                break;
            default:
                break;
        }
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
                break;
            default:
                break;
        }
    }

}
