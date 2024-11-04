using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VolleyPlayer : MonoBehaviour
{
    [SerializeField]
    public RawImage image;
    [SerializeField]
    public TMP_Text blockText;
    [SerializeField]
    public TMP_Text serveText;
    [SerializeField]
    TMP_Text digText;
    [SerializeField]
    TMP_Text passText;
    [SerializeField]
    TMP_Text attackText;
    [SerializeField]
    Button clickableButton;

    public string playerName;
    public Texture illustration;
    public string effectDescription;
    public CardEffect cardEffect;
    public int block;
    public int serve;
    public Vector3Int actionArr;
    public int dig;
    public int pass;
    public int attack;
    public bool isLibero;
    public bool[] isActionAvailable;
    public bool isOrangeTeam;
    public bool isSelected;
    public bool isSelectedTwice;
    public TMP_Text[] actionTexts;

    Game gameScript;
    GameUIManager gameUI;

    public int slotIndex = 0;

    internal void Initialize(VolleyPlayersSO sO)
    {
        playerName = sO.playerName;
        illustration = sO.illustration;
        effectDescription = sO.effectDescription;
        cardEffect = sO.cardEffect;
        block = sO.block;
        serve = sO.serve;
        dig = sO.dig;
        pass = sO.pass;
        attack = sO.attack;
        isLibero = sO.isLibero;
        isOrangeTeam = sO.isOrangeTeam;
        isSelected = false;
        isSelectedTwice = false;

        isActionAvailable = new bool[3];
        isActionAvailable[0] = true;
        isActionAvailable[1] = true;
        isActionAvailable[2] = true;

        actionArr[0] = dig;
        actionArr[1] = pass;
        actionArr[2] = attack;

        gameScript = FindObjectOfType<Game>();
        gameUI = FindObjectOfType<GameUIManager>();
        image.texture = illustration;

        SetSelectable(false);

        if (isLibero)
        {
            blockText.transform.parent.gameObject.SetActive(false);
            serveText.transform.parent.gameObject.SetActive(false);
        }
        else
        {
            blockText.SetText(block.ToString());
            serveText.SetText(serve.ToString());
        }

        digText.SetText(dig.ToString());
        passText.SetText(pass.ToString());
        attackText.SetText(attack.ToString());

        actionTexts = new TMP_Text[3];
        actionTexts[0] = digText;
        actionTexts[1] = passText;
        actionTexts[2] = attackText;
    }

    internal void DeselectActionAnimation(int actionIndex)
    {
        gameUI.DeselectAction(this, actionIndex);
    }

    internal void ResetScaleAction(int actionIndex)
    {
        gameUI.ResetScaleAction(this, actionIndex);
    }

    internal void SelectActionAnimation(int actionIndex)
    {
        gameUI.SelectAction(this, actionIndex);
    }

    // onclick function
    public void CallClickedFunction()
    {
        gameScript.SelectCardButtonFunction(this);
    }

    internal void SetSelectable(bool selectable)
    {
        clickableButton.interactable = selectable;
        gameUI.SetCardSelectable(this, selectable);
    }

    internal void SetActionUnavailable(int i)
    {
        isActionAvailable[i] = false;
        gameUI.SetActionUnavailable(this, i);
    }

    internal void SetIsSelected(bool selected)
    {
        isSelected = selected;
    }

    internal void SetIsSelectedTwice(bool selected)
    {
        isSelectedTwice = selected;
    }

    internal void DeselectCard()
    {
        gameUI.DeselectCard(this);
    }

    internal void SelectBlock()
    {
        gameUI.SelectPlayerBlock(this);
    }    
    
    internal void DeselectBlock()
    {
        gameUI.DeselectPlayerBlock(this);
    }

    internal void SelectServe()
    {
        gameUI.SelectPlayerServe(this);
    }
}

