using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerCardAction
{
    reception,
    pass,
    attack,
    block,
    serv
}

public enum ActionCardEffect
{
    //to populate
}

public enum PlayerCard
{

}

public enum PlayerDeck
{
    Field,
    Sidelines
}

public enum Phase
{
    TeamSelection,
    Block,
    Action,
    Replacement,
    Serve
}
