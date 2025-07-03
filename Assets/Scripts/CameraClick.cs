using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickEventArgs : EventArgs { public Vector2 MouseClick {get; set;} }

public class CameraClick : MonoBehaviour
{
    private Camera cam; // camera reference
    public event EventHandler<ClickEventArgs> OnClick;
    void Start()
    {
        cam = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0)) { // main button
            Vector2 mouseClick = cam.ScreenToWorldPoint(Input.mousePosition);
            if(OnClick!=null){OnClick(this, new ClickEventArgs {MouseClick = mouseClick});}
        }
    }
}
