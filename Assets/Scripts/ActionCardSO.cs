using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenuAttribute(menuName = "Card", fileName = "Action", order = 1)]
public class ActionCardSO : ScriptableObject
{

    public string cardName;
    public Sprite cardIllustration;
    public CardEffect cardEffect;

}
