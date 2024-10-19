using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolleyPlayer : MonoBehaviour
{

    public string playerName;
    public Sprite illustation;
    public string effectDescription;
    public CardEffect cardEffect;
    public int block;
    public int serv;
    public int reception;
    public int pass;
    public int attack;
    public bool isReceptionAvailable;
    public bool isPassAvailable;
    public bool isAttackAvailable;
    public bool isAvailable;
    public bool isLibero;

    internal void Initialize(VolleyPlayersSO sO)
    {
        playerName = sO.playerName;
        illustation = sO.illustation;
        effectDescription = sO.effectDescription;
        cardEffect = sO.cardEffect;
        block = sO.block;
        serv = sO.serv;
        reception = sO.reception;
        pass = sO.pass;
        attack = sO.attack;
    }
}
