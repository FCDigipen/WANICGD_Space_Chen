using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class Gun : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Camera cam;
    public LineRenderer lineRenderer;
    public Transform firePoint;

    [Header("Configurable Values")]
    [SerializeField] private float fireTime;
    [SerializeField] private float fireDistance;


    // Start is called before the first frame update
    void Start()
    {
        DisableLaser();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0)) {
            StartCoroutine(FireLaser());
        }
        UpdateLaser();
    }

    // enables laser & sets fire end
    void EnableLaser() {
        Vector2 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mousePos - (Vector2) firePoint.position).normalized;

        RaycastHit2D hit = Physics2D.Raycast((Vector2) firePoint.position + direction * 0.05f, direction, fireDistance); // direction * 0.05f to prevent intersection with its own collider

        if(hit) {lineRenderer.SetPosition(1, hit.point);}
        else {lineRenderer.SetPosition(1, (Vector2) firePoint.position + direction * fireDistance);}

        lineRenderer.enabled = true;
    }

    // disables laser
    void DisableLaser() {
        lineRenderer.enabled = false;
    }

    // updates laser start position
    void UpdateLaser() {
        lineRenderer.SetPosition(0, firePoint.position);
    }

    // toggles on and off laser fast
    IEnumerator FireLaser() {
        EnableLaser();
        yield return new WaitForSeconds(fireTime);
        DisableLaser();
    }
}
