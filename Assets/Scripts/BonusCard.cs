using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BonusCard : MonoBehaviour
{
    public string cardName;
    public Texture cardIllustration;
    public CardEffect cardEffect;
    public TMP_Text cardEffectText;

    internal void Initialize(BonusCardSO sO)
    {
        cardName = sO.cardName;
        cardIllustration = sO.cardIllustration;
        cardEffect = sO.cardEffect;
        cardEffectText.text = sO.cardEffectDescription;
    }
}
