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

    void Awake()
    {
        Debug.Log("Awake");
        //animator.enabled = false;
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

    internal void UpdatePointText(string text)
    {
        pointText.text = text;
    }
}
