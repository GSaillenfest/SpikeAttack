using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public TeamClass team1;
    public TeamClass team2;

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
    }

    // Start is called before the first frame update
    void Start()
    {
        InitGame();
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

    VolleyPlayer CreateNewPlayer(VolleyPlayersSO sO)
    {
        // create a new game object
        GameObject newPlayer = new GameObject(sO.playerName);
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
