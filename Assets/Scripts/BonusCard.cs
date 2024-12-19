using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BonusCard : MonoBehaviour
{
    [SerializeField]
    public Image image;
    [SerializeField]
    Button clickableButton;
    public string cardName;
    public Texture cardIllustration;
    private EffectType cardEffectType;
    public CardEffect cardEffect;
    public TMP_Text cardEffectText;
    public EffectCardCategory effectCardCategory;
    Game game;
    GameUIManager gameUI;

    private void Start()
    {
        game = FindObjectOfType<Game>();
        gameUI = FindObjectOfType<GameUIManager>();
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
        effectCardCategory = sO.effectCardCategory;
    }
    internal void SetSelectable()
    {
        bool isSelectable = false;
        // TODO Rework this condition
        if (effectCardCategory.ToString() == game.currentPhase.ToString())
        {
            isSelectable = true;
        }
        clickableButton.interactable = isSelectable;
        gameUI.SetCardSelectable(this, isSelectable);
    }
}
