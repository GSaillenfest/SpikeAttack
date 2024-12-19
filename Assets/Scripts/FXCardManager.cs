using System.Collections;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class FXCardManager : MonoBehaviour
{
    [SerializeField]
    private float finalScaleTextFactor;
    [SerializeField]
    private float finalScaleCardFactor;
    [SerializeField]
    private float scaleTextFactor;
    [SerializeField]
    private float scaleCardFactor;
    [SerializeField]
    private float scaleTextTime;
    [SerializeField]
    private float scaleCardTime;
    private GameManager gameManager;
    private TeamClass team1;
    private TeamClass team2;

    [SerializeField]
    VertexGradient orangeGradient = new VertexGradient(
        new Color32(255, 165, 0, 255),  
        new Color32(255, 140, 0, 255), 
        new Color32(255, 69, 0, 255),   
        new Color32(255, 69, 0, 255)
        );
    [SerializeField]
    VertexGradient blueGradient = new VertexGradient(
            new Color32(135, 206, 250, 255), 
            new Color32(135, 206, 235, 255), 
            new Color32(70, 130, 180, 255), 
            new Color32(70, 130, 160, 255)  
        );
    [SerializeField]
    VertexGradient greyGradient = new VertexGradient(
            new Color32(192, 192, 192, 255), 
            new Color32(169, 169, 169, 255), 
            new Color32(128, 128, 128, 255), 
            new Color32(105, 105, 105, 255)  
        );
    [SerializeField]
    VertexGradient darkGreyGradient = new VertexGradient(
            new Color32(192, 192, 192, 255), 
            new Color32(169, 169, 169, 255), 
            new Color32(128, 128, 128, 255), 
            new Color32(105, 105, 105, 255)  
        );
    VertexGradient whiteNonGradient = new VertexGradient(Color.white);
    VertexGradient blackNonGradient = new VertexGradient(Color.black);

    VertexGradient currentTeamGradient;

    Color32 desaturationColor = new Color32(128, 128, 128, 255);
    Color32 unselectableAction = new Color32(55, 55, 55, 215);
    Color32 selectableAction = new Color32(255, 255, 255, 155);

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        team1 = gameManager.teams[0];
        team2 = gameManager.teams[1];
    }

    VertexGradient SelectTeamColorGradient(VolleyPlayer player)
    {
        return player.isOrangeTeam ? orangeGradient : blueGradient;
    }

    public void ShowSelectable(VolleyPlayer playerCard, bool isSelectable)
    {
        playerCard.image.color = isSelectable ? Color.white : desaturationColor;
    }

    public void ShowSelectable(BonusCard bonusCard, bool isSelectable)
    {
        bonusCard.image.color = isSelectable ? Color.white : desaturationColor;
    }

    public void ShowSelected(VolleyPlayer playerCard)
    {
        ApplyCardDropEffect(playerCard.gameObject);
        //BounceCardOnSelection(playerCard.gameObject);
    }

    public void ShowUnselected(VolleyPlayer playerCard)
    {
        if (!playerCard.isSelectedTwice)
        {
            ResetScale(playerCard.gameObject);
        }
    }

    public void ShowSelectedForBlock(VolleyPlayer playerCard, bool applyColor)
    {
        Vector2 initPosMin = Vector2.zero;
        Vector2 initPosMax = Vector2.one;
        Vector2 offset = playerCard.isOrangeTeam ? new Vector2(0.5f, 0) : new Vector2(-0.5f, 0);
        playerCard.GetComponent<RectTransform>().anchorMin = initPosMin + offset;
        playerCard.GetComponent<RectTransform>().anchorMax = initPosMax + offset;
        playerCard.GetComponent<RectTransform>().sizeDelta = Vector2.zero;
        //ApplyCardDropEffect(playerCard.gameObject);

        if (applyColor)
            playerCard.actionValTxtArr[(int)VolleyPlayerAction.block].colorGradient = SelectTeamColorGradient(playerCard);

        BounceOnSelection(playerCard.actionValTxtArr[(int)VolleyPlayerAction.block].gameObject);
    }

    public void ShowUnselectedForBlock(VolleyPlayer playerCard)
    {
        ResetScale(playerCard.actionValTxtArr[(int)VolleyPlayerAction.block].gameObject);
        playerCard.actionValTxtArr[(int)VolleyPlayerAction.block].colorGradient = darkGreyGradient;
        playerCard.GetComponent<RectTransform>().anchorMin = Vector2.zero;
        playerCard.GetComponent<RectTransform>().anchorMax = Vector2.one;
    }

    public void ShowSelectedForServe(VolleyPlayer playerCard)
    {
        Vector2 initPosMin = Vector2.zero;
        Vector2 initPosMax = Vector2.one;
        Vector2 offset = playerCard.isOrangeTeam ? new Vector2(-1, 0) : new Vector2(1, 0);
        playerCard.GetComponent<RectTransform>().anchorMin = initPosMin + offset;
        playerCard.GetComponent<RectTransform>().anchorMax = initPosMax + offset;
        playerCard.GetComponent<RectTransform>().sizeDelta = Vector2.zero;

        //ApplyCardDropEffect(playerCard.gameObject);
        playerCard.actionValTxtArr[(int)VolleyPlayerAction.serve].colorGradient = SelectTeamColorGradient(playerCard);
        BounceOnSelection(playerCard.actionValTxtArr[(int)VolleyPlayerAction.serve].gameObject);
    }

    public void ShowUnselectedForServe(VolleyPlayer playerCard)
    {
        ResetScale(playerCard.actionValTxtArr[(int)VolleyPlayerAction.serve].gameObject);
        playerCard.actionValTxtArr[(int)VolleyPlayerAction.serve].colorGradient = darkGreyGradient;
        playerCard.GetComponent<RectTransform>().anchorMin = Vector2.zero;
        playerCard.GetComponent<RectTransform>().anchorMax = Vector2.one;
    }

    public void ShowSelectedAction(VolleyPlayer player, int actionIndex)
    {

        player.actionValTxtArr[actionIndex].colorGradient = SelectTeamColorGradient(player);
        BounceOnSelection(player.actionValTxtArr[actionIndex].gameObject);
        //add effect around action
    }

    public void ShowUnselectedAction(VolleyPlayer player, int actionIndex)
    {
        player.actionValTxtArr[actionIndex].colorGradient = darkGreyGradient;
        ResetActionScaleOnly(player, actionIndex);
        //add effect around action
    }

    public void ResetActionColorOnly(VolleyPlayer player, int actionIndex)
    {
        player.actionValTxtArr[actionIndex].colorGradient = darkGreyGradient;
        player.actionBgArr[actionIndex].color = selectableAction;
    }

    public void ResetActionScaleOnly(VolleyPlayer player, int actionIndex)
    {
        ResetScale(player.actionValTxtArr[actionIndex].gameObject);
    }

    public void ShowUnselectableAction(VolleyPlayer player, int actionIndex)
    {
        player.actionValTxtArr[actionIndex].colorGradient = greyGradient;
        player.actionBgArr[actionIndex].color = unselectableAction;
    }

    void BounceOnSelection(GameObject go)
    {
        LeanTween.scale(go, scaleTextFactor * Vector3.one, scaleTextTime).setEaseOutBounce().setOnComplete(() => SetTextEndOfAnimScale(go));
    }

    void BounceCardOnSelection(GameObject go)
    {
        LeanTween.scale(go, scaleCardFactor * Vector3.one, scaleCardTime).setEaseOutBounce().setOnComplete(() => SetCardEndOfAnimScale(go));
    }

    void ApplyCardDropEffect(GameObject go)
    {
        Vector3 initialPos = go.transform.parent.position;
        Vector3 newPos = new Vector3(Random.Range(-10, 10), Random.Range(10, 30), 0);
        go.transform.position = initialPos + newPos;
        go.transform.rotation = Quaternion.Euler(0, 0, 10);
        LeanTween.move(go, initialPos, 0.5f).setEaseOutBounce();
        LeanTween.rotateZ(go, 0, 0.1f).setEaseInOutBounce();
    }

    void SetTextEndOfAnimScale(GameObject go)
    {
        LeanTween.scale(go, finalScaleTextFactor * Vector3.one, 0.05f).setEaseInOutQuad();
    }

    void SetCardEndOfAnimScale(GameObject go)
    {
        LeanTween.scale(go, finalScaleCardFactor * Vector3.one, 0.05f).setEaseInOutQuad();
    }

    void ResetScale(GameObject go)
    {
        go.LeanScale(Vector3.one, 0.1f);
    }

}
