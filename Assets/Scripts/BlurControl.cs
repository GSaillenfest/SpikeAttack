using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class BlurControl : MonoBehaviour
{
    [SerializeField]
    Camera mainCamera;
    [SerializeField]
    Camera blurEffectCam;

    int initLayer = -1;
    Transform initParent = null;
    bool isActive = false;

    public void OnEnter(GameObject go)
    {
        //SwitchMainCamera();
        Debug.Log("ApplyBlur");
        initLayer = go.layer;
        initParent = go.transform.parent;
        go.transform.SetParent(transform.GetChild(0), false);
        go.layer = LayerMask.NameToLayer("NoBlur");
        mainCamera.GetComponent<PostProcessLayer>().enabled = true;
    }

    void OnExit(GameObject go)
    {
        Debug.Log("UnApplyBlur");
        go.transform.SetParent(initParent.transform, false);
        go.layer = initLayer;
        mainCamera.GetComponent<PostProcessLayer>().enabled = false;
        //SwitchMainCamera();
        initLayer = -1;
    }

    private void SwitchMainCamera()
    {
        Debug.Log("Change cam"); 
        mainCamera.enabled = blurEffectCam.enabled;
        blurEffectCam.enabled = !mainCamera.enabled;
    }

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
        isActive = !isActive;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
