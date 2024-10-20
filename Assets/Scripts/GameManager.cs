using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public TeamClass team1;
    public TeamClass team2;
    private List<VolleyPlayersSO> playerCardSOs;
    private List<ActionCardSO> actionCardSOs;

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
            VolleyPlayer volleyPlayer = CreateNewPlayer(player);
            volleyPlayer.transform.SetParent(playerCardSet);
            volleyPlayer.transform.position = new Vector3(0, i * volleyPlayer.transform.localScale.y, 0);
            i++;
        }
    }

    VolleyPlayer CreateNewPlayer(VolleyPlayersSO sO)
    {

        // create a new game object
        GameObject newPlayer = Instantiate(playerCardPrefab);

        newPlayer.name = sO.playerName;
        // add the new scritp to that new object
        VolleyPlayer player = newPlayer.AddComponent<VolleyPlayer>();
        player.Initialize(sO);
        return player;
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
