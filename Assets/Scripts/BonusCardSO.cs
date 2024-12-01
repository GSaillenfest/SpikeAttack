using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenuAttribute(menuName = "Card/BonusCard", fileName = "Bonus", order = 1)]
public class BonusCardSO : ScriptableObject
{

    public string cardName;
    public Texture cardIllustration;
    public string cardEffectDescription;
    public CardEffect cardEffect;

}
