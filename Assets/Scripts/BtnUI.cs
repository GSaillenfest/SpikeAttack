using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class BtnUI : MonoBehaviour
{

    [SerializeField]
    Button startBtn;
    [SerializeField]
    Button primaryBtn;
    [SerializeField]
    Button secondaryBtn;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HandleGameStart()
    {
        startBtn.interactable = false;
        startBtn.gameObject.SetActive(false);
        secondaryBtn.gameObject.SetActive(true);
        secondaryBtn.interactable = false;
        primaryBtn.gameObject.SetActive(true);
        primaryBtn.interactable = false;
    }

    public void SetSecondaryBtnInteractable(bool isInteractable)
    {
        secondaryBtn.interactable = isInteractable;
    }

    internal void SetPrimaryBtnInteractable(bool isInteractable)
    {
        primaryBtn.interactable = isInteractable;
    }
}
