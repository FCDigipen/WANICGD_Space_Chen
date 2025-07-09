using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private GameObject explosionVFX;
    [SerializeField] private GameObject startVFX;
    [SerializeField] private LineRenderer lineRenderer;
    public Transform firePoint;
    [Header("Gun Settings")]
    [SerializeField] private int health;
    [SerializeField] private float cooldown;
    [Tooltip("How long after snapping to the player's location does the gun fire")]
    [SerializeField] private float snapDelay;
    [Tooltip("How long the laser lasts after firing")]
    [SerializeField] private float fireTime;
    [SerializeField] private float laserDistance;
    private GameObject player;
    private List<ParticleSystem> startParticles = new List<ParticleSystem>();
    private bool canRotate = true;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player"); // finds player by player object

        // init start particles
        for(int i = 0; i < startVFX.transform.childCount; ++i) {
            ParticleSystem ps = startVFX.transform.GetChild(i).GetComponent<ParticleSystem>();
            if(ps) {startParticles.Add(ps);}
        }

        DisableLaser();

        StartCoroutine(FireLaser());
    }

    // Update is called once per frame
    void Update()
    {
        if(canRotate) {Rotate(player.transform.position);}

        // update laser locations
        lineRenderer.SetPosition(0, firePoint.position);
        startVFX.transform.position = (Vector2) firePoint.position;
    }

    // rotate player dependent on targetPos
    private void Rotate(Vector2 mousePos) {
        Vector2 dir = (mousePos - (Vector2) transform.position).normalized;
        Quaternion qdir = Quaternion.LookRotation(dir, Vector3.right);
        transform.rotation = qdir * Quaternion.Euler(0,-90,0);
    }

    // processes damage (gee wonder)
    public void Damage() {
        --health;
        if(health <= 0) {StartCoroutine(Die());}
    }

    // kills the enemy
    private IEnumerator Die() {
        for(int i = 0; i < explosionVFX.transform.childCount; ++i) {
            ParticleSystem ps = explosionVFX.transform.GetChild(i).GetComponent<ParticleSystem>();
            if(ps) {ps.Play();}
        }
        yield return new WaitForSeconds(0.4f); // s.t. particlesp lay before it kills itself
        Destroy(transform.gameObject);
    }

    private IEnumerator FireLaser() {
        for(int i = 0; i < startParticles.Count; ++i) {startParticles[i].Play();}
        Vector2 direction = (player.transform.position - firePoint.position).normalized; // snap to location

        canRotate = false;

        yield return new WaitForSeconds(snapDelay);
        RaycastHit2D hit = Physics2D.Raycast((Vector2) firePoint.position + direction * 0.05f, direction, laserDistance); // direction * 0.05f to prevent intersection with its own collider

        if(hit) {
            lineRenderer.SetPosition(1, hit.point);
            Debug.Log(hit.transform.name);
        } else {
            lineRenderer.SetPosition(1, (Vector2) firePoint.position + direction * laserDistance);
        }

        lineRenderer.enabled = true;

        yield return new WaitForSeconds(fireTime);

        DisableLaser();

        yield return new WaitForSeconds(cooldown);

        StartCoroutine(FireLaser());
    }

    private void DisableLaser() {
        for(int i = 0; i < startParticles.Count; ++i) {startParticles[i].Stop();}

        lineRenderer.enabled = false;
        canRotate = true;
    }

}
