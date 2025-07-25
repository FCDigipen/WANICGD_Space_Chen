using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private GameObject explosionObj;
    [SerializeField] private GameObject startVFX;
    [SerializeField] private LineRenderer deadlyLaser;
    [SerializeField] private LineRenderer telegraphLaser;
    [SerializeField] private GameObject mapIcon;
    public Transform firePoint;
    [Header("Gun Settings")]
    [SerializeField] private int health;
    [SerializeField] private float cooldown;
    [Tooltip("Offset from fire point to raycast")]
    [SerializeField] private float offset;
    [Tooltip("How long after snapping to the player's location does the gun fire")]
    [SerializeField] private float snapDelay;
    [Tooltip("How long the laser lasts after firing")]
    [SerializeField] private float fireTime;
    [SerializeField] private float laserDistance;
    [SerializeField] private float randomizedDelay;
    private GameObject player;
    private EnemyCounter ec;
    private List<ParticleSystem> startParticles = new List<ParticleSystem>();
    private bool canRotate = true;

    // Start is called before the first frame update
    IEnumerator Start()
    {  
        ec = GameObject.Find("Enemy Counter").GetComponent<EnemyCounter>();
        ec.AddEnemy();

        player = GameObject.Find("Player"); // finds player by player object
        telegraphLaser.enabled = false;

        // init start particles
        for(int i = 0; i < startVFX.transform.childCount; ++i) {
            ParticleSystem ps = startVFX.transform.GetChild(i).GetComponent<ParticleSystem>();
            if(ps) {startParticles.Add(ps);}
        }

        DisableLaser();

        yield return new WaitForSeconds(cooldown + UnityEngine.Random.Range(0f, randomizedDelay));

        StartCoroutine(FireLaser());
    }

    // Update is called once per frame
    void Update()
    {
        if(canRotate && player) {Rotate(player.transform.position);}

        // update laser locations
        deadlyLaser.SetPosition(0, firePoint.position);
        telegraphLaser.SetPosition(0, firePoint.position);
        startVFX.transform.position = (Vector2) firePoint.position;

        if(player) {
            Vector3 dir = transform.position - player.transform.position; // direction vector from player to enemy
            if(dir.magnitude > 45f) { // camera size: make sure it stays on minimap
                mapIcon.transform.position = player.transform.position + 45f * dir.normalized;
            } else {
                mapIcon.transform.position = transform.position;
            }
        }
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
        if(health <= 0) {Die();}
    }

    // kills the enemy
    private void Die()
    {
        ec.RemoveEnemy();
        Destroy(transform.gameObject);

        GameObject explosion = Instantiate(explosionObj, transform.position, Quaternion.identity); // includes sound and visual
        Destroy(explosion, 2f);
    }

    private IEnumerator FireLaser() {
        for(int i = 0; i < startParticles.Count; ++i) {startParticles[i].Play();}
        Vector2 direction = (player.transform.position - firePoint.position).normalized; // snap to location

        RaycastHit2D hit = Physics2D.Raycast((Vector2) firePoint.position + direction * offset, direction, laserDistance); // direction * offset to prevent self-intersection
        if(hit && hit.collider.gameObject.GetComponent<PlayerController>() == null) {
            telegraphLaser.SetPosition(1, hit.point);
        } else {
            telegraphLaser.SetPosition(1, (Vector2) firePoint.position + direction * laserDistance);
        }

        telegraphLaser.enabled = true;
        canRotate = false;

        yield return new WaitForSeconds(snapDelay);

        hit = Physics2D.Raycast((Vector2) firePoint.position + direction * offset, direction, laserDistance); // direction * 0.05f to prevent intersection with its own collider

        if(hit) {
            deadlyLaser.SetPosition(1, hit.point);
            if(hit.collider.gameObject == player) {StartCoroutine(player.GetComponent<PlayerController>().Damage());}
        } else {
            deadlyLaser.SetPosition(1, (Vector2) firePoint.position + direction * laserDistance);
        }

        telegraphLaser.enabled = false;
        deadlyLaser.enabled = true;

        yield return new WaitForSeconds(fireTime);

        DisableLaser();

        yield return new WaitForSeconds(cooldown + UnityEngine.Random.Range(0f, randomizedDelay));

        StartCoroutine(FireLaser());
    }

    private void DisableLaser() {
        for(int i = 0; i < startParticles.Count; ++i) {startParticles[i].Stop();}

        deadlyLaser.enabled = false;
        canRotate = true;
    }

}
