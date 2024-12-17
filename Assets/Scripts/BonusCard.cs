using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BonusCard : MonoBehaviour
{
    public string cardName;
    public Texture cardIllustration;
    private EffectType cardEffectType;
    public CardEffect cardEffect;
    public TMP_Text cardEffectText;
    Game game;

    private void Start()
    {
        game = FindObjectOfType<Game>();
    }

    public void OnButtonClick()
    {
        game.OnBonusSelection(this);
    }

    internal void Initialize(BonusCardSO sO)
    {
        cardName = sO.cardName;
        cardIllustration = sO.cardIllustration;
        cardEffect = sO.cardEffect;
        cardEffectType = sO.cardEffectType;   
        cardEffectText.text = sO.cardEffectDescription;
    }

}
