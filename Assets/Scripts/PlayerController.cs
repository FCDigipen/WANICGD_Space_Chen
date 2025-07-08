using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.EditorTools;
using UnityEngine;
using UnityEngine.Animations;

public class Gun : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Camera cam;
    [SerializeField] private TextMeshProUGUI ammoText;
    public LineRenderer lineRenderer;
    public Transform firePoint;
    [SerializeField] private GameObject startVFX;
    [SerializeField] private GameObject endVFX;

    [Header("Gun Values")]
    [Tooltip("Maximum bullets the player starts off with")]
    [SerializeField] private int maxBullets;
    [Tooltip("Maximum bullets the player can have in the chamber")]
    [SerializeField] private int maxCurrBullets;
    [SerializeField] public float recoil;
    [Tooltip("How long the laser lasts after firing")]
    [SerializeField] private float fireTime;
    [Tooltip("Laser length (visual and actual)")]
    [SerializeField] private float fireDistance;
    [Tooltip("Knockback force experienced by objects")]
    [SerializeField] private float knockback;
    [Tooltip("Minimum delay between successive shots (not including fireTime)")]
    [SerializeField] private float shotDelay;
    [Tooltip("Delay before reloading STARTS")]
    [SerializeField] private float longReload;
    [Tooltip("Delay between consecutive reloads after reloading begins")]
    [SerializeField] private float shortReload;

    private List<ParticleSystem> particles = new List<ParticleSystem>();
    private Rigidbody2D rb;
    private int chamber; // bullets in the chamber
    private int total; // total bullets remaining

    // Start is called before the first frame update
    void Start()
    {
        total = maxBullets;
        chamber = maxCurrBullets; maxBullets -= maxCurrBullets;
        rb = GetComponent<Rigidbody2D>();
        UpdateAmmo();
        FillLists();
        DisableLaser();
    }

    // Update is called once per frame
    void Update()
    {
        Rotate(cam.ScreenToWorldPoint(Input.mousePosition));
        if(Input.GetMouseButtonDown(0) && chamber > 0) {
            StartCoroutine(FireLaser());
        }
        UpdateLaser();
    }

    // rotate player dependent on mousePos.
    private void Rotate(Vector2 mousePos) {
        Vector2 dir = (mousePos - (Vector2) transform.position).normalized;
        Quaternion qdir = Quaternion.LookRotation(dir, Vector3.right);
        transform.rotation = qdir * Quaternion.Euler(0,-90,0);
    }

    // enables laser & sets fire end
    private void EnableLaser() {
        for(int i = 0; i < particles.Count; ++i) {particles[i].Play();}

        Vector2 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mousePos - (Vector2) firePoint.position).normalized;

        rb.velocity = -direction * recoil;

        RaycastHit2D hit = Physics2D.Raycast((Vector2) firePoint.position + direction * 0.05f, direction, fireDistance); // direction * 0.05f to prevent intersection with its own collider

        if(hit) {
            lineRenderer.SetPosition(1, hit.point);
            // apply knockback to rigid body
            // funny enough, this knock back can actually apply to the player LOL
            // i'm keeping it :)
            hit.rigidbody.AddForceAtPosition(direction * knockback, hit.point, ForceMode2D.Impulse);
        }
        else {lineRenderer.SetPosition(1, (Vector2) firePoint.position + direction * fireDistance);}


        endVFX.transform.position = lineRenderer.GetPosition(1);

        lineRenderer.enabled = true;
    }

    // disables laser
    private void DisableLaser() {
        for(int i = 0; i < particles.Count; ++i) {particles[i].Stop();}

        lineRenderer.enabled = false;
    }

    // updates laser start position
    private void UpdateLaser() {
        lineRenderer.SetPosition(0, firePoint.position);
        startVFX.transform.position = (Vector2) firePoint.position;
    }

    // updates ammo text
    private void UpdateAmmo() {
        ammoText.text = $"{chamber} | {total}";
    }

    // toggles on and off laser fast
    private IEnumerator FireLaser() {
        StopCoroutine(StartReload());
        StopCoroutine(Reload());
        --chamber; UpdateAmmo();
        EnableLaser();
        yield return new WaitForSeconds(fireTime);
        DisableLaser();
        StartCoroutine(StartReload());
    }

    private IEnumerator StartReload() {
        yield return new WaitForSeconds(longReload);
        StartCoroutine(Reload());
    }

    private IEnumerator Reload() {
        if(total > 0) {++chamber; --total; UpdateAmmo();}
        yield return new WaitForSeconds(shortReload);
        if(chamber < maxCurrBullets) {StartCoroutine(Reload());}
    }

    private void FillLists() {
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
