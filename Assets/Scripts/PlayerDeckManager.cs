using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeckManager : MonoBehaviour
{

    [SerializeField]
    PlayerDeckOnField playerDeckOnField;

    [SerializeField]
    PlayerDeckOnSidelines playerDeckOnSidelines;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SetActionPhase()
    {

    }

    void SetBlockPhase()
    {

    }

    void SetChangePhase()
    {

    }

    void SetSelectable(Phase phase, PlayerDeck playerDeck)
    {
        // need to define all cases when we need to make some cards selectable or not
        // Replacement >> all except if already replaced
        // Action phases >> if next Action available and not Spike
        // Block phase >> if on the front line and not Spike

        if (playerDeck == PlayerDeck.Field)
        {

        }
        else if (playerDeck == PlayerDeck.Sidelines)
        {

        }


        // foreach (card in deck)
        // {
        //     isSelectable = condition ?
        // }
    }

    void UnsetSelectable(bool condition)
    {

    }
}
