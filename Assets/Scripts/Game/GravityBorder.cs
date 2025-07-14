using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityBorder : MonoBehaviour
{
    // keep objects close, stop ffrom going too far
    [SerializeField] private float radius;
    private Rigidbody2D rb;

    void Start(){rb = transform.GetComponent<Rigidbody2D>();}

    void FixedUpdate()
    {
        float d = transform.position.magnitude;
        if(d > radius) {
            rb.AddForce(-transform.position * Time.fixedDeltaTime, ForceMode2D.Impulse);
        }
    }
}
