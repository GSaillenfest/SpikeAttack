using System.Collections.Generic;
using UnityEngine;
using Unity.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public TeamClass team1;
    public TeamClass team2;
    private List<VolleyPlayersSO> playerCardSOs;
    private List<ActionCardSO> actionCardSOs;

    public RectTransform playerRectTransform;

    [SerializeField]
    Transform playerCardSet;
    [SerializeField]
    Transform actionCardSet;
    [SerializeField]
    GameObject playerCardPrefab;


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
        //test loop
        CreateVolleyPlayerCardsOnAwake();
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
            volleyPlayer.GetComponentsInChildren<RectTransform>()[1].anchoredPosition = new Vector2(i * 68, 0);
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
