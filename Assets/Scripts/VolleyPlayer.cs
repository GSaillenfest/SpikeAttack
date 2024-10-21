using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class VolleyPlayer : MonoBehaviour
{
    [SerializeField]
    RawImage image;
    [SerializeField]
    TMP_Text blockText;
    [SerializeField]
    TMP_Text serveText;
    [SerializeField]
    TMP_Text digText;
    [SerializeField]
    TMP_Text passText;
    [SerializeField]
    TMP_Text attackText;

    public string playerName;
    public Texture illustation;
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

        image.texture = illustation;
        blockText.SetText(block.ToString());
        serveText.SetText(serve.ToString());
        digText.SetText(dig.ToString());
        passText.SetText(pass.ToString());
        attackText.SetText(attack.ToString());
    }
}
