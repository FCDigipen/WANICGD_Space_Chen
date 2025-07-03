using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraClick : MonoBehaviour
{
    // Start is called before the first frame update
    private Camera cam; // camera reference
    void Start()
    {
        cam = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0)) { // main button
            Vector2 mouseClick = cam.ScreenToWorldPoint(Input.mousePosition);
            Debug.Log(mouseClick);
        }
    }
}
