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
    private VolleyPlayer[] selectedPlayerArr;
    private int[] actionArr;
    private TeamClass currentTeam;
    private TeamClass nonPlayingTeam;
    private bool isGameStarted;
    private int powerValue = 0;
    private int previousPowerValue = 0;
    private int blockIndex;
    private int attackIndex;

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
        selectedPlayerArr = new VolleyPlayer[3];
        actionArr = new int[3];
    }


    // Start a regular game (Called by UI Button for now)
    public void StartGame(int team = 1)
    {
        // avoid two start game clicks
        startBtn.interactable = false;
        currentTeam = team == 1 ? team1 : team2;
        currentPhase = Phase.Action;
        StartTurn(currentTeam);
/*        // empty selectedPlayerArr
        selectedPlayerArr[0] = null;
        selectedPlayerArr[1] = null;
        selectedPlayerArr[2] = null;
        // empty actionArr
        actionArr[0] = actionArr[1] = actionArr[2] = 0;
        // reset actionIndex to 0
        actionIndex = 0;
        // set currentTeam to team set by GameManager and nonPlayingTeam
        nonPlayingTeam = GetOppositeTeam(currentTeam);
        // set selectable cards based on available action "Dig"
        SetSelectableCardAction(actionIndex, currentTeam);
        // set all cards unselectable on non playing side
        SetAllSelectableCardAction(false, nonPlayingTeam);
        // reset power text on UI
        gameUI.UpdatePowerText(powerValue);
        // change current team on UI
        SetValidateButtonInteractable(false);*/
    }

    public void StartTurn(TeamClass team)
    {
        SetActionPhase(team);
    }

    private void SetActionPhase(TeamClass team)
    {
        Debug.Log("Action phase");
        currentPhase = Phase.Action;
        EmptySelectedPlayerArr();
        actionArr[0] = actionArr[1] = actionArr[2] = 0;
        actionIndex = 0;
        currentTeam = team;
        nonPlayingTeam = GetOppositeTeam(currentTeam);
        SetSelectableCardAction(actionIndex, currentTeam);
        SetAllSelectableCardAction(false, nonPlayingTeam);
        gameUI.UpdatePowerText(powerValue);
        SetValidateButtonInteractable(false);
    }

    private void EmptySelectedPlayerArr()
    {
        selectedPlayerArr[0] = null;
        selectedPlayerArr[1] = null;
        selectedPlayerArr[2] = null;
    }

    TeamClass GetOppositeTeam(TeamClass team)
    {
        return team == team1 ? team2 : team1;
    }

    void SetBlockPhase()
    {
        Debug.Log("Block phase");
        foreach (GameObject slot in currentTeam.deckOnField.deckSlots)
        {
            bool isSelectable = (slot.GetComponentInChildren<VolleyPlayer>().slotIndex >= 2 && slot.GetComponentInChildren<VolleyPlayer>().slotIndex <= 4);
            slot.GetComponentInChildren<VolleyPlayer>().SetSelectable(isSelectable);
        }
        SetValidateButtonInteractable(false);
    }

    public void SelectAction(VolleyPlayer selected)
    {
        if (actionIndex == 0)
        {
            actionArr[actionIndex] = selected.actionArr[actionIndex];
            selectedPlayerArr[actionIndex] = selected;
            selected.SelectActionAnimation(actionIndex);
            selected.SetIsSelected(true);
            actionIndex++;
            SetSelectableCardAction(actionIndex, currentTeam);
        }
        else if (actionIndex < 3)
        {
            if (selectedPlayerArr[actionIndex - 1] == selected)
            {
                selectedPlayerArr[actionIndex - 1] = null;
                actionArr[actionIndex - 1] = 0;
                selected.DeselectActionAnimation(actionIndex - 1);
                selected.SetIsSelected(false);
                actionIndex--;
                SetSelectableCardAction(actionIndex, currentTeam);
            }
            else
            {
                actionArr[actionIndex] = selected.actionArr[actionIndex];
                selectedPlayerArr[actionIndex] = selected;
                selected.SelectActionAnimation(actionIndex);
                if (!selected.isSelected)
                    selected.SetIsSelected(true);
                else selected.SetIsSelectedTwice(true);
                actionIndex++;
                SetSelectableCardAction(actionIndex, currentTeam);
            }
        }
        else if (actionIndex == 3)
        {
            if (selectedPlayerArr[actionIndex - 1] == selected)
            {
                selectedPlayerArr[actionIndex - 1] = null;
                actionArr[actionIndex - 1] = 0;
                selected.DeselectActionAnimation(actionIndex - 1);
                if (selected.isSelectedTwice)
                    selected.SetIsSelectedTwice(false);
                else selected.SetIsSelected(false);
                SetValidateButtonInteractable(false);
                gameUI.DeactivateValidateButton();
                actionIndex--;
                SetSelectableCardAction(actionIndex, currentTeam);
            }
        }

        if (actionIndex == 3)
        {
            SetValidateButtonInteractable(true);
        }

        UpdatePowerValue();
    }

    public void SelectBlockCard(VolleyPlayer player, int slotIndex)
    {
        Debug.Log(selectedPlayerArr[0]);
        if (selectedPlayerArr[0] == null)
        {
            selectedPlayerArr[0] = player;
            blockIndex = slotIndex;
            SetAllSelectableCardAction(false, currentTeam);
            selectedPlayerArr[0].SetSelectable(true);
            selectedPlayerArr[0].SelectBlock();
            SetValidateButtonInteractable(true);
        }
        else
        {
            selectedPlayerArr[0].DeselectBlock();
            selectedPlayerArr[0] = null;
            blockIndex = 0;
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

    void SetSelectableCardAction(int actionIndex, TeamClass team)
    {
        if (actionIndex == 3)
        {
            foreach (GameObject slot in team.deckOnField.deckSlots)
            {
                if (slot.GetComponentInChildren<VolleyPlayer>() == selectedPlayerArr[actionIndex - 1])
                {
                    slot.GetComponentInChildren<VolleyPlayer>().SetSelectable(true);
                }
                else
                {
                    slot.GetComponentInChildren<VolleyPlayer>().SetSelectable(false);
                }
            }
        }
        else
        {
            foreach (GameObject slot in team.deckOnField.deckSlots)
            {
                if (slot.GetComponentInChildren<VolleyPlayer>().isActionAvailable[actionIndex] || (actionIndex > 0 && slot.GetComponentInChildren<VolleyPlayer>() == selectedPlayerArr[actionIndex - 1]))
                {
                    slot.GetComponentInChildren<VolleyPlayer>().SetSelectable(true);
                }
                else
                {
                    slot.GetComponentInChildren<VolleyPlayer>().SetSelectable(false);
                }
            }
        }
    }

    void SetAllSelectableCardAction(bool selectable, TeamClass team)
    {
        foreach (GameObject slot in team.deckOnField.deckSlots)
        {
            slot.GetComponentInChildren<VolleyPlayer>().SetSelectable(selectable);
        }
    }

    void ValidateActions()
    {
        SetAllSelectableCardAction(false, currentTeam);
        for (int i = 0; i < selectedPlayerArr.Length; i++)
        {
            selectedPlayerArr[i].SetActionUnavailable(i);
            selectedPlayerArr[i].ResetScaleAction(i);
            selectedPlayerArr[i].DeselectCard();
            selectedPlayerArr[i].SetIsSelected(false);
            selectedPlayerArr[i].SetIsSelectedTwice(false);
        }
        EmptySelectedPlayerArr();
        SetValidateButtonInteractable(false);
        ChangePhase(Phase.Block);
    }

    void ValidateBlockSelection()
    {
        SetAllSelectableCardAction(false, currentTeam);
        selectedPlayerArr[0].DeselectCard();
        selectedPlayerArr[0].DeselectBlock();
        EndTurn();
    }

    void EndTurn()
    {
        previousPowerValue = powerValue;
        powerValue = 0;
        gameManager.EndTurn();
    }

    public void SelectCardButtonFunction(VolleyPlayer player, int slotIndex)
    {
        Debug.Log("Card clicked on " + currentPhase);
        switch (currentPhase)
        {
            case Phase.TeamSelection:
                break;
            case Phase.Block:
                SelectBlockCard(player, slotIndex);
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
