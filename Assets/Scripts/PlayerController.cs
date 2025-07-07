using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Camera cam;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    // called whenever the mouse is clciekd
    void OnClick(Vector2 mousePos) {
        Debug.Log("Mouse Clicked");
    }

    // called whenever the mouse is moved
    void OnMove(Vector2 mousePos) {
        Debug.Log("Mouse Moved");
    }
}
