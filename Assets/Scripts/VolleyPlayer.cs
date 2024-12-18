using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VolleyPlayer : MonoBehaviour
{
    [SerializeField]
    public RawImage image;

    [SerializeField]
    TMP_Text blockValText;
    [SerializeField]
    TMP_Text serveValText;
    [SerializeField]
    TMP_Text effectDescrText;
    [SerializeField]
    TMP_Text digValText;
    [SerializeField]
    TMP_Text passValText;
    [SerializeField]
    TMP_Text attackValText;
    [SerializeField]
    TMP_Text blockText;
    [SerializeField]
    TMP_Text serveText;
    [SerializeField]
    TMP_Text effectText;
    [SerializeField]
    TMP_Text digText;
    [SerializeField]
    TMP_Text passText;
    [SerializeField]
    TMP_Text attackText;
    [SerializeField]
    Image blockBg;
    [SerializeField]
    Image serveBg;
    [SerializeField]
    Image effectBg;
    [SerializeField]
    Image digBg;
    [SerializeField]
    Image passBg;
    [SerializeField]
    Image attackBg;
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
    public TMP_Text[] actionValTxtArr;
    public TMP_Text[] actionTxtArr;
    public Image[] actionBgArr;
    public int slotIndex = 10;

    Game gameScript;
    GameUIManager gameUI;


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
            blockValText.transform.parent.gameObject.SetActive(false);
            serveValText.transform.parent.gameObject.SetActive(false);
        }
        else
        {
            blockValText.SetText(block.ToString());
            serveValText.SetText(serve.ToString());
        }

        if (effectDescription != "")
        {
            effectDescrText.text = effectDescription;
        }
        else
        {
            effectDescrText.transform.parent.gameObject.SetActive(false);
        }

        digValText.SetText(dig.ToString());
        passValText.SetText(pass.ToString());
        attackValText.SetText(attack.ToString());

        actionValTxtArr = new TMP_Text[] {
            digValText,
            passValText,
            attackValText,
            blockValText,
            serveValText,
        };

        actionTxtArr = new TMP_Text[3];
        actionTxtArr[0] = digText;
        actionTxtArr[1] = passText;
        actionTxtArr[2] = attackText;

        actionBgArr = new Image[] {
            digBg,
            passBg,
            attackBg,
            blockBg,
            serveBg,
        };


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
        gameScript.HandleCardButtonFunction(this);
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

    internal void SetActionAvailable(int actionIndex)
    {
        isActionAvailable[actionIndex] = true;
        gameUI.SetActionAvailable(this, actionIndex);
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

    internal void SelectBlock(bool applyColor)
    {
        gameUI.SelectPlayerBlock(this, applyColor);
    }

    internal void DeselectBlock()
    {
        gameUI.DeselectPlayerBlock(this);
    }

    internal void SelectServe()
    {
        gameUI.SelectPlayerServe(this);
    }

    internal void DeselectServe()
    {
        gameUI.DeselectPlayerServe(this);
    }

    internal void ResetActions()
    {
        SetActionAvailable(0);
        SetActionAvailable(1);
        SetActionAvailable(2);
    }
}

