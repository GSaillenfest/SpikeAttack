using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public TeamClass team1;
    public TeamClass team2;
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
                volleyPlayer.SetActive(true);
            }
            else if (volleyPlayer.GetComponentInChildren<VolleyPlayer>(true).isLibero == false)
            {
                volleyPlayer.transform.SetParent(slots[slotIndex].transform, false);
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
            Debug.Log(volleyPlayer.name + volleyPlayer.GetComponentsInChildren<RectTransform>()[1].anchoredPosition);
            //volleyPlayer.GetComponentsInChildren<RectTransform>()[1].anchoredPosition = new Vector2(i * 68, 0);
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

    public void EndTurn()
    {

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
}
