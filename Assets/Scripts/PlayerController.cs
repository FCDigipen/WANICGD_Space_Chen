using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Camera cam;

    [SerializeField] public float recoil;

    private Vector2 playerVelocity;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mousePos = cam.ScreenToWorldPoint(Input.mousePosition); // mouse position in world space
        OnMove(mousePos);
        if(Input.GetMouseButtonDown(0)) {OnClick(mousePos);}
    }

    // frame-rate independent update function
    void FixedUpdate()
    {
        transform.position += (Vector3) playerVelocity * Time.fixedDeltaTime;
    }

    // called whenever the mouse is clciekd
    void OnClick(Vector2 mousePos) {
        Vector2 dir = Rotate(mousePos);
        playerVelocity = -dir * recoil;
    }

    // called whenever the mouse is moved
    void OnMove(Vector2 mousePos) {
        Rotate(mousePos);
    }

    // rotate player dependent on mousePos. returns direction vector for OnMove
    private Vector2 Rotate(Vector2 mousePos) {
        Vector2 dir = (mousePos - (Vector2) transform.position).normalized;
        float angle = Vector2.Angle(dir, (Vector2) transform.right); // d(angle)
        transform.Rotate(Vector3.forward * angle);
        return dir;
    }
}
