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
    private int blockIndex;
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
        blockIndex = nonAttributed;
        attackIndex = nonAttributed;
    }


    // Start a regular game (Called by UI Button for now)
    public void StartGame(int team = 1)
    {
        // avoid two start game clicks
        startBtn.interactable = false;
        currentTeam = team == 1 ? team1 : team2;
        currentPhase = Phase.Action;
        StartTurn();
    }

    public void StartTurn()
    {
        SetActionPhase(currentTeam);
    }

    private void SetActionPhase(TeamClass team)
    {
        Debug.Log("Action phase");
        currentPhase = Phase.Action;
        EmptySelectedCardSlots();
        actionArr[0] = actionArr[1] = actionArr[2] = 0;
        actionIndex = 0;
        currentTeam = team;
        nonPlayingTeam = GetOppositeTeam(currentTeam);
        SetSelectableCardAction();
        SetAllSelectableCardAction(nonPlayingTeam, false);
        gameUI.UpdatePowerText(powerValue);
        SetValidateButtonInteractable(false);
    }

    private void EmptySelectedCardSlots()
    {
        selectedCardSlots[0] = nonAttributed;
        selectedCardSlots[1] = nonAttributed;
        selectedCardSlots[2] = nonAttributed;
    }

    TeamClass GetOppositeTeam(TeamClass team)
    {
        return team == team1 ? team2 : team1;
    }

    void SetBlockPhase()
    {
        currentTeam.SetBlockPhase();
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

    public void SelectBlockCard(VolleyPlayer player)
    {
        if (selectedCardSlots[0] == nonAttributed)
        {
            selectedCardSlots[0] = player.slotIndex;
            blockIndex = player.slotIndex;
            SetAllSelectableCardAction(currentTeam, false);
            player.SetSelectable(true);
            player.SelectBlock();
            SetValidateButtonInteractable(true);
        }
        else
        {
            player.DeselectBlock();
            selectedCardSlots[0] = nonAttributed;
            blockIndex = nonAttributed;
            SetBlockPhase();
        }
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

    void SetSelectableCardAction()
    {
        currentTeam.SetSelectableCardAction(actionIndex, actionArr, selectedCardSlots);
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
        EmptySelectedCardSlots();
        SetValidateButtonInteractable(false);
        attackIndex = selectedCardSlots[2];
        ChangePhase(Phase.Block);
    }

    void ValidateBlockSelection()
    {
        SetAllSelectableCardAction(currentTeam, false);
        currentTeam.ValidateBlock(selectedCardSlots[0]);
        EndTurn();
    }

    void EndTurn()
    {
        previousPowerValue = powerValue;
        powerValue = 0;
        gameManager.EndTurn();
    }

    public void SelectCardButtonFunction(VolleyPlayer player)
    {
        switch (currentPhase)
        {
            case Phase.TeamSelection:
                break;
            case Phase.Block:
                SelectBlockCard(player);
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
            case Phase.Block:
                ValidateBlockSelection();
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

    void ChangePhase(Phase phase)
    {
        currentPhase = phase;
        switch (currentPhase)
        {
            case Phase.TeamSelection:
                break;
            case Phase.Block:
                SetBlockPhase();
                break;
            case Phase.Action:
                SetActionPhase(nonPlayingTeam);
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
