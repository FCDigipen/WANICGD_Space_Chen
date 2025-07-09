using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private int health;
    [SerializeField] private GameObject explosionVFX;
    private GameObject player;
    private List<ParticleSystem> particles = new List<ParticleSystem>();

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player"); // finds player by player object

        for(int i = 0; i < explosionVFX.transform.childCount; ++i) {
            ParticleSystem ps = explosionVFX.transform.GetChild(i).GetComponent<ParticleSystem>();
            if(ps) {particles.Add(ps);}
        }
    }

    // Update is called once per frame
    void Update()
    {
        Rotate(player.transform.position);
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
        for(int i = 0; i < particles.Count; ++i) {particles[i].Play();}
        yield return new WaitForSeconds(0.5f);
        Destroy(transform.gameObject);
    }
}
