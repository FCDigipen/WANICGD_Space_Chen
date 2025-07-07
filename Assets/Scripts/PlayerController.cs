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
        Rotate(mousePos);
    }

    // called whenever the mouse is moved
    void OnMove(Vector2 mousePos) {
        Debug.Log("Mouse Moved");
        Vector2 dir = Rotate(mousePos);
    }

    // rotate player dependent on mousePos. returns direction vector for OnMove
    private Vector2 Rotate(Vector2 mousePos) {
        Vector2 dir = (mousePos - (Vector2) transform.position).normalized;
        float angle = Vector2.Angle(dir, new Vector2(1f,0f)); // base angle
        transform.Rotate(Vector3.forward * angle);
        return dir;
    }
}
