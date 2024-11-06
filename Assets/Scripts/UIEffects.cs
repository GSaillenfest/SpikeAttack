using System.Collections;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class UIEffects : MonoBehaviour
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

    VertexGradient orangeGradient = new VertexGradient(
        new Color32(255, 165, 0, 255),  // Orange clair en haut à gauche
        new Color32(255, 140, 0, 255),  // Orange moyen en haut à droite
        new Color32(255, 69, 0, 255),   // Orange foncé en bas à gauche
        new Color32(255, 69, 0, 255)// Orange foncé en bas à droite
        );
    VertexGradient blueGradient = new VertexGradient(
            new Color32(135, 206, 250, 255), // Bleu clair en haut à gauche
            new Color32(135, 206, 235, 255), // Bleu ciel en haut à droite
            new Color32(70, 130, 180, 255),  // Bleu acier en bas à gauche
            new Color32(70, 130, 160, 255)   // Bleu acier foncé en bas à droite
        );
    VertexGradient greyGradient = new VertexGradient(
            new Color32(192, 192, 192, 255), // Gris clair en haut à gauche
            new Color32(169, 169, 169, 255), // Gris moyen en haut à droite
            new Color32(128, 128, 128, 255), // Gris foncé en bas à gauche
            new Color32(105, 105, 105, 255)  // Gris très foncé en bas à droite
        );
    VertexGradient whiteNonGradient = new VertexGradient(Color.white);

    VertexGradient currentTeamGradient;

    Color32 desaturationColor = new Color32(128, 128, 128, 255);

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
        Vector3 offset = playerCard.isOrangeTeam ? new Vector3(20, 0, 0) : new Vector3(-20, 0, 0);
        playerCard.gameObject.transform.parent.position += offset;
        ApplyCardDropEffect(playerCard.gameObject);

        if (applyColor) 
            playerCard.blockText.colorGradient = SelectTeamColorGradient(playerCard);

        BounceOnSelection(playerCard.blockText.gameObject);
    }

    public void ShowUnselectedForBlock(VolleyPlayer playerCard)
    {
        Vector3 offset = playerCard.isOrangeTeam ? new Vector3(20, 0, 0) : new Vector3(-20, 0, 0);
        ResetScale(playerCard.blockText.gameObject);
        playerCard.blockText.colorGradient = whiteNonGradient;
        playerCard.gameObject.transform.parent.position -= offset;
    }

    public void ShowSelectedForServe(VolleyPlayer playerCard)
    {
        Vector3 offset = playerCard.isOrangeTeam ? new Vector3(-20, 0, 0) : new Vector3(+20, 0, 0);
        playerCard.gameObject.transform.parent.position += offset;
        ApplyCardDropEffect(playerCard.gameObject);
        playerCard.serveText.colorGradient = SelectTeamColorGradient(playerCard);
        BounceOnSelection(playerCard.serveText.gameObject);
    }

    public void ShowUnselectedForServe(VolleyPlayer playerCard)
    {
        Vector3 offset = playerCard.isOrangeTeam ? new Vector3(-20, 0, 0) : new Vector3(+20, 0, 0);
        ResetScale(playerCard.serveText.gameObject);
        playerCard.serveText.colorGradient = whiteNonGradient;
        playerCard.gameObject.transform.position -= offset;
    }

    public void ShowSelectedAction(VolleyPlayer player, int actionIndex)
    {

        player.actionTexts[actionIndex].colorGradient = SelectTeamColorGradient(player);
        BounceOnSelection(player.actionTexts[actionIndex].gameObject);
        //add effect around action
    }

    public void ShowUnselectedAction(VolleyPlayer player, int actionIndex)
    {
        player.actionTexts[actionIndex].colorGradient = whiteNonGradient;
        ResetActionScaleOnly(player, actionIndex);
        //add effect around action
    }

    public void ResetActionScaleOnly(VolleyPlayer player, int actionIndex)
    {
        ResetScale(player.actionTexts[actionIndex].gameObject);
    }

    public void ShowUnselectableAction(VolleyPlayer player, int actionIndex)
    {
        player.actionTexts[actionIndex].colorGradient = greyGradient;
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
