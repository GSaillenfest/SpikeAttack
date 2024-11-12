using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class BlurControl : MonoBehaviour
{
    int initLayer = -1;
    Transform initParent = null;
    bool isActive = false;

    public void OnEnter(GameObject go)
    {
        initLayer = go.layer;
        initParent = go.transform.parent;
        go.transform.SetParent(transform.GetChild(0), false);
        go.layer = LayerMask.NameToLayer("NoBlur");
    }

    void OnExit(GameObject go)
    {
        Debug.Log("UnApplyBlur");
        go.transform.SetParent(initParent.transform, false);
        go.layer = initLayer;
        initLayer = -1;
    }

    public void CallBlurEffect(GameObject target, GameObject[] exceptions = null)
    {
        if (isActive)
        {
           // OnExit(target);
        }
        else
        {
           // OnEnter(target);
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
