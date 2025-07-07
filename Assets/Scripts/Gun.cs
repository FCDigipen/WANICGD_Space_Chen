using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] private Camera cam;
    public LineRenderer lineRenderer;
    public Transform firePoint;


    // Start is called before the first frame update
    void Start()
    {
        DisableLaser();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0)) {
            FireLaser();
        }
    }

    // enables laser
    void EnableLaser() {

    }

    // disables laser
    void DisableLaser() {

    }

    // toggles on and off laser fast
    void FireLaser() {
        
    }
}
