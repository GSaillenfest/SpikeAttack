using UnityEngine;

public class Game : MonoBehaviour
{

    private TeamClass team1;
    private TeamClass team2;
    private int turn;
    private VolleyPlayer[] lastSelected;
    private Vector3Int actionArr;

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
    }

    // Start is called before the first frame update
    void Start()
    {
        lastSelected = new VolleyPlayer[3];
        lastSelected[0] = null;
        lastSelected[1] = null;
        lastSelected[2] = null;
        actionArr = Vector3Int.zero;
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
            lastSelected[actionIndex] = selected;
            actionIndex++;
        }
        else if (actionIndex < 3)
        {
            if (lastSelected[actionIndex - 1] == selected)
            {
                lastSelected[actionIndex - 1] = null;
                actionArr[actionIndex - 1] = 0;
                actionIndex--;
            }
            else
            {
                actionArr[actionIndex] = selected.actionArr[actionIndex];
                lastSelected[actionIndex] = selected;
                actionIndex++;
            }
        }
        else if (actionIndex == 3)
        {   
            if (lastSelected[actionIndex - 1] == selected)
            {
                lastSelected[actionIndex - 1] = null;
                actionArr[actionIndex - 1] = 0;
                actionIndex--;
            }
        }

        Debug.Log(actionIndex);
        Debug.Log(actionArr);
        //stop action selection state
        if (actionIndex == 3)
        {
            Debug.Log("Activate Validate button");
            //SetValidate(true);
        }

        // SetAvailableAction
    }

    public void SelectCardEffect()
    {
        /*if (isSelected)
        card.getComponent<Effect>().Activate();*/
    }
}
