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
    Button validateBtn;

    private int turn;
    private VolleyPlayer[] selectedPlayerArr;
    private int[] actionArr;
    private TeamClass currentTeam;
    private TeamClass nonPlayingTeam;
    private bool isGameStarted;
    private int powerValue = 0;

    public int actionIndex = 0;


    public Game(TeamClass t1, TeamClass t2)
    {
        team1 = t1;
        team2 = t2;
        turn = 0;
    }

    public void PlayerTurn()
    {
        turn++;

        TeamClass temp = nonPlayingTeam;
        nonPlayingTeam = currentTeam;
        currentTeam = temp;

    }

    // Called by UI Button for now
    public void StartGame()
    {
        // avoid two start game clicks
        if (isGameStarted) return;
        selectedPlayerArr = new VolleyPlayer[3];
        selectedPlayerArr[0] = null;
        selectedPlayerArr[1] = null;
        selectedPlayerArr[2] = null;
        actionArr = new int[3];
        actionArr[0] = actionArr[1] = actionArr[2] = 0;
        currentTeam = team1;
        nonPlayingTeam = team2;
        SetSelectableCardAction(actionIndex, currentTeam);
        SetAllSelectableCardAction(false, nonPlayingTeam);
        gameUI.UpdatePowerText(powerValue);
        gameUI.ChangeCurrentTeam(currentTeam);
        validateBtn.interactable = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SelectAction(VolleyPlayer selected)
    {
        if (actionIndex == 0)
        {
            actionArr[actionIndex] = selected.actionArr[actionIndex];
            selectedPlayerArr[actionIndex] = selected;
            selected.SelectActionAnimation(actionIndex);
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
                actionIndex--;
                SetSelectableCardAction(actionIndex, currentTeam);
            }
            else
            {
                actionArr[actionIndex] = selected.actionArr[actionIndex];
                selectedPlayerArr[actionIndex] = selected;
                selected.SelectActionAnimation(actionIndex);
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
                validateBtn.interactable = false;
                gameUI.DeactivateValidateButton();
                actionIndex--;
                SetSelectableCardAction(actionIndex, currentTeam);
            }
        }


        if (actionIndex == 3)
        {
            validateBtn.interactable = true;
        }

        UpdatePowerValue();
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

    public void ValidateActions()
    {
        SetAllSelectableCardAction(false, currentTeam);
        for (int i = 0; i < selectedPlayerArr.Length; i++)
        {
            selectedPlayerArr[i].SetActionUnavailable(i);
        }
    }

}
