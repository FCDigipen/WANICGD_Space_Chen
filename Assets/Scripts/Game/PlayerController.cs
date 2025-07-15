using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Camera cam;
    public LineRenderer lineRenderer;
    public Transform firePoint;
    [SerializeField] private GameObject startVFX;
    [SerializeField] private GameObject endVFX;
    [SerializeField] private GameObject explode;
    [SerializeField] private GameObject scene;
    [SerializeField] private GameObject ShotCounter;
    [SerializeField] private GameObject goingToExplodeSFXObject;
    [SerializeField] private GameObject goingToExplodeTextObject;

    [Header("Gun Values")]
    [SerializeField] public float recoil;
    [Tooltip("Laser length (visual and actual)")]
    [SerializeField] private float fireDistance;
    [Tooltip("Knockback force experienced by objects")]
    [SerializeField] private float knockback;

    [Header("Other")]
    [Tooltip("How long the laser lasts after firing")]
    [SerializeField] private float fireTime;
    [SerializeField] private float respawnTime;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float borderRadius;
    [Tooltip("How long after leaving the border do you explode")]
    [SerializeField] private float explosionTime;

    private List<ParticleSystem> particles = new List<ParticleSystem>();
    private Rigidbody2D rb;
    private ShotCounter shots;
    private StateManager sm;
    private AudioSource shootAudio;
    private AudioSource goingToExplodeSFX;
    private TextMeshProUGUI goingToExplodeText;
    private bool exploding;
    private bool exploded = false;
    private float explosionTimer; // displayed on the screen

    // Start is called before the first frame update
    void Start()
    {
        sm = scene.GetComponent<StateManager>();
        rb = GetComponent<Rigidbody2D>();
        shootAudio = GetComponent<AudioSource>();
        goingToExplodeSFX = goingToExplodeSFXObject.GetComponent<AudioSource>();
        goingToExplodeText = goingToExplodeTextObject.GetComponent<TextMeshProUGUI>();
        shots = ShotCounter.GetComponent<ShotCounter>();
        FillLists();
        DisableLaser();
    }

    // Update is called once per frame
    void Update()
    {
        if (sm.getState == StateManager.GameState.PLAYING && !exploded) // TODO: fix start particle beign spawned after lazer
        {
            Rotate(cam.ScreenToWorldPoint(Input.mousePosition));
            if(Input.GetMouseButtonDown(0) && sm.getState == StateManager.GameState.PLAYING) {
                StartCoroutine(FireLaser());
            }
        }
        UpdateLaser();
    }

    void FixedUpdate()
    {
        // clamp velocity
        rb.velocity = Vector2.ClampMagnitude(rb.velocity, maxSpeed);

        // check for explosion
        if (!exploded)
        {
            float d = transform.position.magnitude;
            if (d > borderRadius && !exploding)
            {
                exploding = true;
                explosionTimer = explosionTime;
                goingToExplodeSFX.Play();
                goingToExplodeText.enabled = true;
                TimeSpan t = TimeSpan.FromSeconds(explosionTimer);
                goingToExplodeText.text = $"time={t.Minutes:00}:{t.Seconds:00}:{t.Milliseconds:000};";
            }
            else if (exploding)
            {
                explosionTimer -= Time.fixedDeltaTime;
                if (explosionTimer <= 0)
                {
                    goingToExplodeSFX.Stop();
                    exploded = true;
                    StartCoroutine(Damage()); // die
                }
            }
            if (d <= borderRadius && !exploding)
            {// not exploding or in explosion radius
                exploding = false;
                goingToExplodeSFX.Stop();
            }
        }
    }

    // rotate player dependent on mousePos.
    private void Rotate(Vector2 mousePos) {
        Vector2 dir = (mousePos - (Vector2) transform.position).normalized;
        Quaternion qdir = Quaternion.LookRotation(dir, Vector3.right);
        transform.rotation = qdir * Quaternion.Euler(0,-90,0);
    }

    // enables laser & sets fire end
    private void EnableLaser() {
        shots.UpdateShots();
        for(int i = 0; i < particles.Count; ++i) {particles[i].Play();}

        Vector2 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mousePos - (Vector2) transform.position).normalized;

        rb.AddForce(-direction * recoil, ForceMode2D.Impulse);

        RaycastHit2D hit = Physics2D.Raycast((Vector2) firePoint.position + direction * 0.05f, direction, fireDistance); // direction * 0.05f to prevent intersection with its own collider

        if(hit) {
            shootAudio.Play();
            lineRenderer.SetPosition(1, hit.point);
            // apply knockback to rigid body
            // funny enough, this knock back can actually apply to the player LOL
            // i'm keeping it :)
            hit.rigidbody.AddForceAtPosition(direction * knockback, hit.point, ForceMode2D.Impulse);

            EnemyController enemy = hit.collider.gameObject.GetComponent<EnemyController>();
            if(enemy) {enemy.Damage();}
        }
        else {lineRenderer.SetPosition(1, (Vector2) firePoint.position + direction * fireDistance);}


        GameObject endVFXobj = Instantiate(endVFX, lineRenderer.GetPosition(1), Quaternion.identity);
        Destroy(endVFXobj, 2f);

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

    // toggles on and off laser fast
    private IEnumerator FireLaser() {
        EnableLaser();
        yield return new WaitForSeconds(fireTime);
        DisableLaser();
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

    public IEnumerator Damage() {
        GameObject d = Instantiate(explode, transform.position, Quaternion.identity);
        exploded = true; // disable other actions
        GetComponent<Transform>().localScale = Vector3.zero; // hide
        yield return new WaitForSeconds(respawnTime);
        sm.Lose();
        Destroy(d);
        Destroy(gameObject);
    }
    


}
