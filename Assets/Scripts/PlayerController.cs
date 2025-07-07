using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Camera cam;

    private Vector2 lastMousePos;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition); // mouse position in world space
        if(mousePos != lastMousePos) {OnMove(mousePos); lastMousePos = mousePos;}
        if(Input.GetMouseButtonDown(0)) {OnClick(mousePos);}
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
