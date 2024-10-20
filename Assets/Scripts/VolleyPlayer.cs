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
    public int serve;
    public int dig;
    public int pass;
    public int attack;
    public bool isLibero;
    public bool isReceptionAvailable;
    public bool isPassAvailable;
    public bool isAttackAvailable;
    public bool isAvailable;

    internal void Initialize(VolleyPlayersSO sO)
    {
        playerName = sO.playerName;
        illustation = sO.illustation;
        effectDescription = sO.effectDescription;
        cardEffect = sO.cardEffect;
        block = sO.block;
        serve = sO.serve;
        dig = sO.dig;
        pass = sO.pass;
        attack = sO.attack;
        isLibero = sO.isLibero;
    }
}
