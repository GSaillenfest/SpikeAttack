using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public TeamClass team1;
    public TeamClass team2;
    public TeamClass currentTeam;
    public List<TeamClass> teams = new();
    private List<VolleyPlayersSO> playerCardSOs;
    private List<ActionCardSO> actionCardSOs;
    private List<GameObject> volleyPlayersOrange = new List<GameObject>();
    private List<GameObject> volleyPlayersBlue = new List<GameObject>();

    public RectTransform playerRectTransform;

    [SerializeField]
    Transform playerCardSet;
    [SerializeField]
    Transform actionCardSet;
    [SerializeField]
    GameObject playerCardPrefab;
    [SerializeField]
    GameObject playerOne;
    [SerializeField]
    GameObject playerTwo;

    Game game;
    private int turn;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        playerCardSOs = GetComponentInChildren<CardSet>().playerCardSOs;
        actionCardSOs = GetComponentInChildren<CardSet>().actionCardSOs;
        game = GetComponent<Game>();
        teams.Add(team1);
        teams.Add(team2);
    }

    // Start is called before the first frame update
    void Start()
    {
        InitGame();
        //test loop to instantiate volley player cards
        CreateVolleyPlayerCardsOnAwake();

        //test function to populate a player team. To be refactored.
        PopulateTeam(playerOne, volleyPlayersOrange);
        PopulateTeam(playerTwo, volleyPlayersBlue);
    }

    private void PopulateTeam(GameObject player, List<GameObject> volleyPlayers)
    {
        GameObject[] slots = player.GetComponentInChildren<PlayerDeckOnField>().deckSlots;
        int slotIndex = 1;
        foreach (GameObject volleyPlayer in volleyPlayers)
        {
            if (slotIndex == 6) return;
            if (volleyPlayer.GetComponentInChildren<VolleyPlayer>(true).isLibero == true && slots[0].transform.childCount == 0)
            {
                volleyPlayer.transform.SetParent(slots[0].transform, false);
                volleyPlayer.GetComponentInChildren<VolleyPlayer>(true).slotIndex = 0;
                volleyPlayer.SetActive(true);
            }
            else if (volleyPlayer.GetComponentInChildren<VolleyPlayer>(true).isLibero == false)
            {
                volleyPlayer.transform.SetParent(slots[slotIndex].transform, false);
                volleyPlayer.GetComponentInChildren<VolleyPlayer>(true).slotIndex = slotIndex;
                volleyPlayer.SetActive(true);
                slotIndex++;
            }
        }
    }

    void InitGame()
    {
        // to implement, first try with premade teams
        //team1 = new TeamClass();
        //team2 = new TeamClass();
        turn = 0;
        currentTeam = team1;
    }

    public void ChoseTeamPlayer()
    {

    }

    void CreateVolleyPlayerCardsOnAwake()
    {
        int i = 0;
        foreach (var player in playerCardSOs)
        {
            GameObject volleyPlayer = CreateNewPlayer(player);
            volleyPlayer.transform.SetParent(playerCardSet);
            volleyPlayer.GetComponentsInParent<RectTransform>()[0].anchoredPosition = Vector2.zero;
            volleyPlayer.GetComponentsInParent<RectTransform>()[0].anchorMin = 0.5f * Vector2.one;
            volleyPlayer.GetComponentsInParent<RectTransform>()[0].anchorMax = 0.5f * Vector2.one;
            if (volleyPlayer.GetComponentInChildren<VolleyPlayer>().isOrangeTeam)
                volleyPlayersOrange.Add(volleyPlayer);
            else
                volleyPlayersBlue.Add(volleyPlayer);
            volleyPlayer.gameObject.SetActive(false);
            i++;
        }
    }

    GameObject CreateNewPlayer(VolleyPlayersSO sO)
    {
        // create a new game object
        GameObject newPlayer = Instantiate(playerCardPrefab);

        newPlayer.name = sO.playerName;
        newPlayer.GetComponentInChildren<VolleyPlayer>().Initialize(sO);
        return newPlayer;
    }

    public void PlayTurn()
    {
        Debug.Log("Turn :" + turn);
        turn++;
    }

    void StartTurn(TeamClass team)
    {
        SetCurrentTeam(team);
        game.StartTurn();
    }

    public void EndTurn()
    {
        Debug.Log("End Turn");
        StartTurn(SwitchTeam());
    }

    public void EndPoint()
    {
        Debug.Log("End point");
    }

    public void EndMatch()
    {
        Debug.Log("End match");
    }

    // Update is called once per frame
    void Update()
    {

    }

    TeamClass SwitchTeam()
    {
        return currentTeam = currentTeam == team1 ? team2 : team1;
    }

    void SetCurrentTeam(TeamClass team)
    {
        currentTeam = team;
    }

}
