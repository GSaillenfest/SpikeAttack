using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamClass : MonoBehaviour
{

    public List<GameObject> playerList;
    public PlayerDeckOnField deckOnField;

    public TeamClass()
    {
        playerList = new List<GameObject>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void AddPlayerToTeam(GameObject playerCard)
    {
        playerList.Add(playerCard);
    }
}
