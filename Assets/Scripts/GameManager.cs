using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public TeamClass team1;
    public TeamClass team2;
    public TeamClass currentTeam;
    public List<TeamClass> teams = new();
    private List<VolleyPlayersSO> playerCardSOs;
    private List<BonusCardSO> bonusCardSOs;
    private List<GameObject> volleyPlayersOrange = new List<GameObject>();
    private List<GameObject> volleyPlayersBlue = new List<GameObject>();

    //public RectTransform playerRectTransform;

    [SerializeField]
    Transform playerCardSet;
    [SerializeField]
    Transform bonusCardSet;
    [SerializeField]
    GameObject playerCardPrefab;
    [SerializeField]
    GameObject bonusCardPrefab;
    [SerializeField]
    GameObject playerOne;
    [SerializeField]
    GameObject playerTwo;    
    [SerializeField]
    CardSet cardSet;
    [SerializeField]
    BonusCardSetHandler bonusDeckHandler;

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

        playerCardSOs = cardSet.playerCardSOs;
        bonusCardSOs = cardSet.bonusCardSOs;
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
        CreateBonusCardsOnAwake();

        //test function to populate a player team. To be refactored.
        PopulateTeam(playerOne, volleyPlayersOrange);
        PopulateTeam(playerTwo, volleyPlayersBlue);

        DrawBonusCards(team1, 3);
        DrawBonusCards(team2, 3);
    }

    private void DrawBonusCards(TeamClass team, int nbOfCards)
    {
        team.AddBonusCard(nbOfCards);
    }

    private void PopulateTeam(GameObject player, List<GameObject> volleyPlayers)
    {
        GameObject[] fieldSlots = player.GetComponentInChildren<PlayerDeckOnField>(true).deckSlots;
        GameObject[] sideSlots = player.GetComponentInChildren<PlayerDeckOnSidelines>(true).deckSlots;

        int slotIndex = 1;

        foreach (GameObject volleyPlayer in volleyPlayers)
        {
            if (slotIndex < 6)
            {

                if (volleyPlayer.GetComponentInChildren<VolleyPlayer>(true).isLibero == true && fieldSlots[0].transform.childCount == 0)
                {
                    volleyPlayer.transform.SetParent(fieldSlots[0].transform, true);
                    ResizeCard(volleyPlayer);
                    volleyPlayer.GetComponentInChildren<VolleyPlayer>(true).slotIndex = 0;
                    volleyPlayer.SetActive(true);
                }
                else if (volleyPlayer.GetComponentInChildren<VolleyPlayer>(true).isLibero == false)
                {
                    volleyPlayer.transform.SetParent(fieldSlots[slotIndex].transform, true);
                    ResizeCard(volleyPlayer);
                    volleyPlayer.GetComponentInChildren<VolleyPlayer>(true).slotIndex = slotIndex;
                    volleyPlayer.SetActive(true);
                    slotIndex++;
                }
                else if (volleyPlayer.GetComponentInChildren<VolleyPlayer>(true).isLibero)
                {
                    volleyPlayer.transform.SetParent(sideSlots[0].transform, true);
                    ResizeCard(volleyPlayer);
                    volleyPlayer.GetComponentInChildren<VolleyPlayer>(true).slotIndex = slotIndex;
                    volleyPlayer.SetActive(true);
                }
            }
            else
            {
                volleyPlayer.transform.SetParent(sideSlots[slotIndex - 6 + 1].transform, true);
                ResizeCard(volleyPlayer);
                volleyPlayer.GetComponentInChildren<VolleyPlayer>(true).slotIndex = slotIndex;
                volleyPlayer.SetActive(true);
                slotIndex++;
            }
        }
    }

    void ResizeCard(GameObject volleyPlayer)
    {
        RectTransform rectTransform = volleyPlayer.GetComponent<RectTransform>();
        if (rectTransform != null)
        {
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
            rectTransform.offsetMin = Vector2.zero;
            rectTransform.offsetMax = Vector2.zero;
            rectTransform.localScale = Vector3.one;
            rectTransform.anchoredPosition3D = Vector3.zero;
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
    void CreateBonusCardsOnAwake()
    {
        foreach (BonusCardSO bonusCard in bonusCardSOs)
        {
            GameObject bonusCardInstance = CreateNewBonusCard(bonusCard);
            bonusDeckHandler.OrganizeCard(bonusCardInstance);
            //To remove
            //bonusCardInstance.SetActive(false);
        }
    }

    void CreateVolleyPlayerCardsOnAwake()
    {
        int i = 0;
        foreach (var player in playerCardSOs)
        {
            GameObject volleyPlayer = CreateNewPlayer(player);
            volleyPlayer.transform.SetParent(playerCardSet);

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

    GameObject CreateNewBonusCard(BonusCardSO sO)
    {
        GameObject newBonusCard = Instantiate(bonusCardPrefab);

        newBonusCard.name = sO.cardName;
        newBonusCard.GetComponentInChildren<BonusCard>().Initialize(sO);
        return newBonusCard;
    }

    public void StartGame()
    {
        SetCurrentTeam(team1);
        game.StartGame(currentTeam);
        team1.OnStart();
        team2.OnStart();
    }

    public void PlayTurn()
    {
        Debug.Log("Turn :" + turn);
        turn++;
    }


    public void EndPoint()
    {
        Debug.Log("End point");
    }

    public void EndMatch()
    {
        Debug.Log("End match");
    }

    void SetCurrentTeam(TeamClass team)
    {
        currentTeam = team;
        game.SetCurrentTeam(currentTeam);
    }


}
