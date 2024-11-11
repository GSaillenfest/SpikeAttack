using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

public class BlurControl : MonoBehaviour
{
    [SerializeField]
    Transform parent;
    [SerializeField]
    RawImage blurImage;
    [SerializeField]
    RectTransform rectTransform;

    bool isActive = false;

    public void CallBlurEffect(GameObject go)
    {
        Debug.Log(isActive);
        if (isActive)
        {
            OnExit(go);
        }
        else
        {
            OnEnter(go);
        }
        Rescale();
        isActive = !isActive;
    }

    public void OnEnter(GameObject go)
    {
        gameObject.transform.SetParent(go.transform);
        blurImage.enabled = true;
    }

    void OnExit(GameObject go)
    {
        blurImage.enabled = false;
        gameObject.transform.SetParent(parent);
    }

    void Rescale()
    {
        rectTransform.anchorMin = Vector2.zero;
        rectTransform.anchorMax = Vector2.one;
        rectTransform.offsetMin = Vector2.zero;
        rectTransform.offsetMax = Vector2.zero;
        rectTransform.localScale = Vector3.one;
    }


}
