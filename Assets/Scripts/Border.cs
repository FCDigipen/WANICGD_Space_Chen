using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ensures that given rigidbody will never exceed boundary
// TODO: make it more exciting :(

public class Border : MonoBehaviour
{
    [Tooltip("Min/Max x value the given rigid body should have")]
    [SerializeField] public Vector2 xBoundary;
    [Tooltip("Min/Max y value the given rigid body should have")]
    [SerializeField] public Vector2 yBoundary;
    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start() { rb = GetComponent<Rigidbody2D>(); }

    void FixedUpdate()
    {
        rb.position = new Vector2(Mathf.Clamp(rb.position.x, xBoundary.x, xBoundary.y), Mathf.Clamp(rb.position.y, yBoundary.x, yBoundary.y));
        if ((rb.position.x <= xBoundary.x && rb.velocity.x < 0) || (rb.position.x >= xBoundary.y && rb.velocity.x > 0)) { rb.velocity = new Vector2(-rb.velocity.x, rb.velocity.y); }
        if ((rb.position.y <= yBoundary.x && rb.velocity.y < 0) || (rb.position.y >= yBoundary.y && rb.velocity.y > 0)) { rb.velocity = new Vector2(rb.velocity.x, -rb.velocity.y); }
    }
}
