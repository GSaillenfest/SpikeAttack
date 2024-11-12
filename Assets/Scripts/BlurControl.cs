using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

/*    public void OnButtonClick()
    {
        CallBlurEffect(GameObject.Find("General UI"), GameObject.Find("Libero"));
    }*/

    public void CallBlurEffect(GameObject blurTargetGameObject, GameObject[] excludedGameObject = null)
    {
        if (isActive)
        {
            OnExit(blurTargetGameObject, excludedGameObject);
        }
        else
        {
            OnEnter(blurTargetGameObject, excludedGameObject);
        }
        Rescale();
        isActive = !isActive;
    }

    public void OnEnter(GameObject blurTargetGameObject, GameObject[] excludedGameObject = null)
    {
        gameObject.transform.SetParent(blurTargetGameObject.transform);
        blurImage.enabled = true;
        foreach (GameObject child in excludedGameObject)
        {
            if (child.TryGetComponent(out Canvas canvas))
            {
                canvas.overrideSorting = true;
                canvas.sortingLayerName = "NoBlur";
            }
        }
    }

    void OnExit(GameObject blurTargetGameObject, GameObject[] excludedGameObject = null)
    {
        blurImage.enabled = false;
        gameObject.transform.SetParent(parent);
        foreach (GameObject child in excludedGameObject)
        {
            if (child.TryGetComponent(out Canvas canvas))
            {
                canvas.sortingLayerName = string.Empty;
                canvas.overrideSorting = false;
            }
        }
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
