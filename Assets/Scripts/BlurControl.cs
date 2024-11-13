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

    public void CallBlurEffect(GameObject blurTargetGameObject, int orderInLayer = 0, GameObject[] excludedGameObject = null)
    {
        if (isActive)
        {
            OnExit(blurTargetGameObject, orderInLayer, excludedGameObject);
        }
        else
        {
            OnEnter(blurTargetGameObject, orderInLayer, excludedGameObject);
        }
        Rescale();
        isActive = !isActive;
    }

    public void OnEnter(GameObject blurTargetGameObject, int orderInLayer, GameObject[] excludedGameObject = null)
    {
        gameObject.transform.SetParent(blurTargetGameObject.transform);
        blurImage.enabled = true;
        blurImage.GetComponent<Canvas>().sortingOrder = orderInLayer;
        if (excludedGameObject != null)
        {
            foreach (GameObject child in excludedGameObject)
            {
                if (child.TryGetComponent(out Canvas canvas))
                {
                    canvas.overrideSorting = true;
                    canvas.sortingLayerName = "NoBlur";
                    canvas.sortingOrder = orderInLayer + 1;
                }
            }
        }
    }

    void OnExit(GameObject blurTargetGameObject, int orderInLayer, GameObject[] excludedGameObject = null)
    {
        blurImage.GetComponent<Canvas>().sortingOrder = 0;
        blurImage.enabled = false;
        gameObject.transform.SetParent(parent);
        if (excludedGameObject != null)
        {
            foreach (GameObject child in excludedGameObject)
            {
                if (child.TryGetComponent(out Canvas canvas))
                {
                    canvas.sortingLayerName = string.Empty;
                    canvas.overrideSorting = false;
                    canvas.sortingOrder = 0;
                }
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
