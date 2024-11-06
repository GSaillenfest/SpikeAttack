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
    BlockSelection,
    BlockResolution,
    Action,
    Replacement,
    Serve,
    Inactive,
}

public enum Side
{
    Orange,
    Blue,
    Neutral,
}
