using System;
using TMPro;
using UnityEngine;
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
    [SerializeField]
    Button clickableButton;

    public string playerName;
    public Texture illustation;
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
    public TMP_Text[] actionTexts;

    Game gameScript;
    GameUIManager gameUI;

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
        isOrangeTeam = sO.isOrangeTeam;

        isActionAvailable = new bool[3];
        isActionAvailable[0] = true;
        isActionAvailable[1] = true;
        isActionAvailable[2] = true;

        actionArr[0] = dig;
        actionArr[1] = pass;
        actionArr[2] = attack;

        gameScript = FindObjectOfType<Game>();
        gameUI = FindObjectOfType<GameUIManager>();

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

        image.texture = illustation;
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

    internal void SelectActionAnimation(int actionIndex)
    {
        gameUI.SelectAction(this, actionIndex);
    }

    public void SelectAction()
    {
        gameScript.SelectAction(this);
    }

    internal void SetSelectable(bool selectable)
    {
        clickableButton.interactable = selectable;
    }

    internal void SetActionUnavailable(int i)
    {
        isActionAvailable[i] = false;
        gameUI.SetActionUnavailable(this, i);
    }
}
