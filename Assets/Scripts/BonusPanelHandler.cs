using System;
using UnityEngine;

public class BonusPanelHandler : MonoBehaviour
{

    [SerializeField]
    float activeMinPosX;
    [SerializeField]
    float activeMaxPosX;
    [SerializeField]
    RectTransform targetRectT;
    [SerializeField]
    float initMinPosX;
    [SerializeField]
    float initMaxPosX;
    [SerializeField]
    private Animator animator;
    
    float targetMinPosX;
    float targetMaxPosX;
    bool isActive = false;

    private void Start()
    {
        initMinPosX = targetRectT.anchorMin.x;
        initMaxPosX = targetRectT.anchorMax.x;
        targetMinPosX = initMinPosX;
        targetMaxPosX = initMaxPosX;
    }

    public void OnButtonClick()
    {
        Debug.Log("Button clicked");

        if (!isActive)
        {
            targetMinPosX = activeMinPosX;
            targetMaxPosX = activeMaxPosX;
        }
        else
        {
            targetMinPosX = initMinPosX;
            targetMaxPosX = initMaxPosX;
        }
        isActive = !isActive;

        ActivatePanel();
    }

    private void ActivatePanel()
    {
        AnimatePanel();
        if (isActive)
        {
            //SetCardSelectable();
        }
    }

    public void DesactivatePanel()
    {
        if (isActive)
        {
            isActive = false;
            AnimatePanel();
        }
    }

    void AnimatePanel()
    {
        animator.SetTrigger("Activate");
    }
}
