using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidRandom : MonoBehaviour
{
    [Header("Randomized Values")]
    [SerializeField] private float min_rot;
    [SerializeField] private float max_rot;
    [SerializeField] private float min_angv;
    [SerializeField] private float max_angv;
    [SerializeField] private Vector2 min_v;
    [SerializeField] private Vector2 max_v;
    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.rotation = Random.Range(min_rot, max_rot);
        rb.angularVelocity = Random.Range(min_angv, max_angv);
        rb.velocity = Vector2.right * Random.Range(min_v.x, max_v.x) + Vector2.up * Random.Range(min_v.y, max_v.y);
    }
}
