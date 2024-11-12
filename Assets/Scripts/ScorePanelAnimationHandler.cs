using UnityEngine;
using TMPro;


public class ScorePanelAnimationHandler : MonoBehaviour
{
    [SerializeField]
    Animator animator;
    [SerializeField]
    ParticleSystem particles;
    [SerializeField]
    private GameObject panel;
    [SerializeField]
    private TextMeshProUGUI pointText;
    
    private GameUIManager gameUI;

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

    void Awake()
    {
        //animator.enabled = false;
        panel.SetActive(false);
        gameUI = FindObjectOfType<GameUIManager>();
    }

    public void StartAnim(Side side)
    {
        panel.SetActive(true);
        //animator.enabled = true;

        if (side == Side.Orange)
        {
            animator.SetTrigger("DisplayScoreOrange");
        }
        else animator.SetTrigger("DisplayScoreBlue");

        particles.gameObject.SetActive(true);
        particles.Play();
    }

    public void StopAnimation()
    {
        Debug.Log("Stop anim");
        //animator.enabled = false;
        panel.SetActive(false);
        gameUI.EndTemporization();
    }

    public void UpdateScore()
    {
        gameUI.UpdateScore();
    }

    public void StopParticles()
    {
        particles.Stop(true, stopBehavior: ParticleSystemStopBehavior.StopEmittingAndClear);
        particles.gameObject.SetActive(false);
    }

    internal void UpdateScoreText(Side side, string text)
    {
        pointText.colorGradient = side == Side.Orange ? orangeGradient : blueGradient;
        pointText.text = text;
    }
}
