using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "NewPlayerCard", menuName = "Card/Player", order = 0)]
public class VolleyPlayersSO : ScriptableObject
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

}
