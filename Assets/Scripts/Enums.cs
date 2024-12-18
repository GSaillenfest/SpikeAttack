using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum VolleyPlayerAction
{
    dig = 0,
    pass = 1,
    attack = 2,
    block = 3,
    serve = 4,
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
