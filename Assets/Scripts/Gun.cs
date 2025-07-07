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
    [SerializeField] private GameObject startVFX;
    [SerializeField] private GameObject endVFX;

    [Header("Configurable Values")]
    [SerializeField] private float fireTime;
    [SerializeField] private float fireDistance;

    private List<ParticleSystem> particles = new List<ParticleSystem>();

    // Start is called before the first frame update
    void Start()
    {
        FillLists();
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
        for(int i = 0; i < particles.Count; ++i) {particles[i].Play();}

        Vector2 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mousePos - (Vector2) firePoint.position).normalized;

        RaycastHit2D hit = Physics2D.Raycast((Vector2) firePoint.position + direction * 0.05f, direction, fireDistance); // direction * 0.05f to prevent intersection with its own collider

        if(hit) {lineRenderer.SetPosition(1, hit.point);}
        else {lineRenderer.SetPosition(1, (Vector2) firePoint.position + direction * fireDistance);}

        endVFX.transform.position = lineRenderer.GetPosition(1);

        lineRenderer.enabled = true;
    }

    // disables laser
    void DisableLaser() {
        for(int i = 0; i < particles.Count; ++i) {particles[i].Stop();}

        lineRenderer.enabled = false;
    }

    // updates laser start position
    void UpdateLaser() {
        lineRenderer.SetPosition(0, firePoint.position);
        startVFX.transform.position = (Vector2) firePoint.position;
    }

    // toggles on and off laser fast
    IEnumerator FireLaser() {
        EnableLaser();
        yield return new WaitForSeconds(fireTime);
        DisableLaser();
    }

    void FillLists() {
        for(int i = 0; i < startVFX.transform.childCount; ++i) {
            ParticleSystem ps = startVFX.transform.GetChild(i).GetComponent<ParticleSystem>();
            if(ps) {particles.Add(ps);}
        }
        for(int i = 0; i < endVFX.transform.childCount; ++i) {
            ParticleSystem ps = endVFX.transform.GetChild(i).GetComponent<ParticleSystem>();
            if(ps) {particles.Add(ps);}
        }
    }

}
